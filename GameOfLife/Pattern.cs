namespace GameOfLife
{
    /// <summary>
    /// Represents a Game of Life pattern with metadata and cell state data.
    /// </summary>
    public class Pattern
    {
        /// <summary>
        /// Name of the pattern (from RLE #N comment).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description or comment about the pattern (from RLE #C comments).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Author of the pattern (from RLE #O comment).
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Width of the pattern bounding box.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the pattern bounding box.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Rule string (typically "B3/S23" for standard Conway's Life).
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 2D array representing the pattern state. True = alive, False = dead.
        /// Indexed as [x, y] where x is horizontal and y is vertical.
        /// </summary>
        public bool[,] Cells { get; set; }

        /// <summary>
        /// Creates a new empty pattern with default rule.
        /// </summary>
        public Pattern()
        {
            Name = string.Empty;
            Description = string.Empty;
            Author = string.Empty;
            Rule = "B3/S23";
        }

        /// <summary>
        /// Creates a pattern from a grid state array.
        /// </summary>
        public Pattern(bool[,] cells, string name = "")
        {
            Width = cells.GetLength(0);
            Height = cells.GetLength(1);
            Cells = (bool[,])cells.Clone();
            Name = name;
            Description = string.Empty;
            Author = string.Empty;
            Rule = "B3/S23";
        }
    }
}
