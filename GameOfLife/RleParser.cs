using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    /// <summary>
    /// Parser and writer for RLE (Run Length Encoded) Game of Life pattern files.
    /// </summary>
    public static class RleParser
    {
        /// <summary>
        /// Parses an RLE format string into a Pattern object.
        /// </summary>
        /// <param name="rleContent">The RLE file content as a string.</param>
        /// <returns>A Pattern object containing the parsed data.</returns>
        /// <exception cref="FormatException">Thrown when RLE format is invalid.</exception>
        public static Pattern Parse(string rleContent)
        {
            if (string.IsNullOrWhiteSpace(rleContent))
                throw new FormatException("RLE content is empty.");

            var pattern = new Pattern();
            var lines = rleContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var patternLines = new List<string>();

            // Parse header comments and extract metadata
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("#N ", StringComparison.OrdinalIgnoreCase))
                {
                    pattern.Name = trimmedLine.Substring(3).Trim();
                }
                else if (trimmedLine.StartsWith("#C ", StringComparison.OrdinalIgnoreCase) ||
                         trimmedLine.StartsWith("#c ", StringComparison.OrdinalIgnoreCase))
                {
                    pattern.Description += trimmedLine.Substring(3).Trim() + " ";
                }
                else if (trimmedLine.StartsWith("#O ", StringComparison.OrdinalIgnoreCase))
                {
                    pattern.Author = trimmedLine.Substring(3).Trim();
                }
                else if (trimmedLine.StartsWith("#"))
                {
                    // Other comment types, ignore
                    continue;
                }
                else if (trimmedLine.StartsWith("x ", StringComparison.OrdinalIgnoreCase))
                {
                    // Header line: x = width, y = height, rule = rulestring
                    ParseHeaderLine(trimmedLine, pattern);
                }
                else if (!string.IsNullOrWhiteSpace(trimmedLine))
                {
                    // Pattern data lines
                    patternLines.Add(trimmedLine);
                }
            }

            pattern.Description = pattern.Description.Trim();

            if (pattern.Width == 0 || pattern.Height == 0)
                throw new FormatException("Pattern dimensions not found in RLE header.");

            // Combine all pattern lines into one string
            string patternData = string.Join("", patternLines);

            // Decode the RLE pattern data
            pattern.Cells = DecodePattern(patternData, pattern.Width, pattern.Height);

            return pattern;
        }

        /// <summary>
        /// Parses the RLE header line (x = width, y = height, rule = rulestring).
        /// </summary>
        private static void ParseHeaderLine(string headerLine, Pattern pattern)
        {
            // Example: "x = 36, y = 9, rule = B3/S23"
            var parts = headerLine.Split(',');

            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                if (trimmed.StartsWith("x", StringComparison.OrdinalIgnoreCase))
                {
                    var value = trimmed.Split('=')[1].Trim();
                    pattern.Width = int.Parse(value);
                }
                else if (trimmed.StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    var value = trimmed.Split('=')[1].Trim();
                    pattern.Height = int.Parse(value);
                }
                else if (trimmed.StartsWith("rule", StringComparison.OrdinalIgnoreCase))
                {
                    var value = trimmed.Split('=')[1].Trim();
                    pattern.Rule = value;
                }
            }
        }

        /// <summary>
        /// Decodes RLE pattern data into a 2D boolean array.
        /// </summary>
        private static bool[,] DecodePattern(string patternData, int width, int height)
        {
            var cells = new bool[width, height];
            int x = 0, y = 0;
            int runCount = 0;

            // Remove the terminating '!' if present
            patternData = patternData.TrimEnd('!');

            for (int i = 0; i < patternData.Length; i++)
            {
                char c = patternData[i];

                if (char.IsDigit(c))
                {
                    // Build up run count
                    runCount = runCount * 10 + (c - '0');
                }
                else if (c == 'b' || c == 'B' || c == '.')
                {
                    // Dead cells
                    int count = runCount == 0 ? 1 : runCount;
                    x += count; // Skip dead cells
                    runCount = 0;
                }
                else if (c == 'o' || c == 'O' || c == 'A')
                {
                    // Alive cells (O or A for compatibility)
                    int count = runCount == 0 ? 1 : runCount;
                    for (int j = 0; j < count; j++)
                    {
                        if (x < width && y < height)
                            cells[x, y] = true;
                        x++;
                    }
                    runCount = 0;
                }
                else if (c == '$')
                {
                    // End of line
                    int count = runCount == 0 ? 1 : runCount;
                    y += count;
                    x = 0;
                    runCount = 0;
                }
                else if (char.IsWhiteSpace(c))
                {
                    // Ignore whitespace
                    continue;
                }
            }

            return cells;
        }

        /// <summary>
        /// Writes a Pattern object to RLE format string.
        /// </summary>
        /// <param name="pattern">The pattern to encode.</param>
        /// <returns>RLE formatted string.</returns>
        public static string Write(Pattern pattern)
        {
            var sb = new StringBuilder();

            // Write metadata comments
            if (!string.IsNullOrWhiteSpace(pattern.Name))
                sb.AppendLine($"#N {pattern.Name}");

            if (!string.IsNullOrWhiteSpace(pattern.Author))
                sb.AppendLine($"#O {pattern.Author}");

            if (!string.IsNullOrWhiteSpace(pattern.Description))
            {
                // Split long descriptions into multiple comment lines
                var descLines = pattern.Description.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in descLines)
                    sb.AppendLine($"#C {line.Trim()}");
            }

            // Write header line
            sb.AppendLine($"x = {pattern.Width}, y = {pattern.Height}, rule = {pattern.Rule}");

            // Encode pattern data
            string encodedPattern = EncodePattern(pattern.Cells, pattern.Width, pattern.Height);
            sb.Append(encodedPattern);
            sb.AppendLine("!");

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a 2D boolean array into RLE pattern string.
        /// </summary>
        private static string EncodePattern(bool[,] cells, int width, int height)
        {
            var sb = new StringBuilder();
            int lineLength = 0;
            const int maxLineLength = 70; // Keep lines readable

            for (int y = 0; y < height; y++)
            {
                int runCount = 0;
                bool currentState = false;

                for (int x = 0; x < width; x++)
                {
                    bool cellState = cells[x, y];

                    if (x == 0)
                    {
                        currentState = cellState;
                        runCount = 1;
                    }
                    else if (cellState == currentState)
                    {
                        runCount++;
                    }
                    else
                    {
                        // Write the run
                        string run = FormatRun(runCount, currentState);
                        sb.Append(run);
                        lineLength += run.Length;

                        // Start new run
                        currentState = cellState;
                        runCount = 1;
                    }
                }

                // Write final run of the line
                if (runCount > 0)
                {
                    string run = FormatRun(runCount, currentState);
                    sb.Append(run);
                    lineLength += run.Length;
                }

                // Add line terminator (except for last line)
                if (y < height - 1)
                {
                    sb.Append('$');
                    lineLength++;

                    // Break line if too long
                    if (lineLength > maxLineLength)
                    {
                        sb.AppendLine();
                        lineLength = 0;
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Formats a run of cells into RLE notation.
        /// </summary>
        private static string FormatRun(int count, bool isAlive)
        {
            char c = isAlive ? 'o' : 'b';

            if (count == 1)
                return c.ToString();
            else
                return $"{count}{c}";
        }
    }
}
