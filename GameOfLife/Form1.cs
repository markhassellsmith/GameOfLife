using GameOfLife;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WinFormsFractal;

namespace TP14_JeudelaVie
{
    /// <summary>
    /// Main form for Conway's Game of Life simulation.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Number of cells per horizontal line in the grid.
        /// </summary>
        private const int squarePerLine = 150;   //60

        /// <summary>
        /// Number of cells per vertical column in the grid.
        /// </summary>
        private const int squarePerColumn = 72;  //36

        /// <summary>
        /// Horizontal offset from the window edge for grid positioning.
        /// </summary>
        private static int positionOffsetX = 30;

        /// <summary>
        /// Vertical offset from the window edge for grid positioning.
        /// </summary>
        private static int positionOffsetY = 30;

        /// <summary>
        /// Current color index in the spectrum palette for cell generation coloring.
        /// </summary>
        private static int colornumber = 0;

        /// <summary>
        /// Current generation/iteration number.
        /// </summary>
        private static int TickNumber = 0;

        /// <summary>
        /// Number of currently alive cells in the grid.
        /// </summary>
        private static int AliveCount = 0;

        /// <summary>
        /// Number of alive cells in the previous generation for stability detection.
        /// </summary>
        private static int PreviousAliveCount = 0;

        /// <summary>
        /// Tracks whether the simulation is in start state (true) or stop state (false) for spacebar toggle.
        /// </summary>
        private bool isStart = true;

        /// <summary>
        /// Minimum number of neighbors required for a cell to remain alive (Game of Life rule).
        /// </summary>
        private int ViabilityLowerBound = 2;

        /// <summary>
        /// Maximum number of neighbors required for a cell to remain alive (Game of Life rule).
        /// </summary>
        private int ViabilityUpperBound = 3;

        /// <summary>
        /// Bitmap used for rendering the entire grid.
        /// </summary>
        private Bitmap gridBitmap;

        private PictureBox gridDisplay;

        private static bool[,] squaresState = new bool[squarePerLine, squarePerColumn];
        private static bool[,] squaresFutureState = new bool[squarePerLine, squarePerColumn];
        private static bool[,] squaresPastState = new bool[squarePerLine, squarePerColumn];
        private static int squareSize = 10;  //20
        private static int cellSpacing;

        // stability detection (same alive count for N consecutive ticks)
        private int stableConsecutiveCount = 0;

        private const int stableRequiredTicks = 5;

        // per-cell color to preserve birth color (age)
        private Color[,] cellColor = new Color[squarePerLine, squarePerColumn];

        /// <summary>
        /// Form used for rendering the entire grid.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (isStart)
                {
                    runToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    stopToolStripMenuItem_Click(sender, e);
                }
                isStart = !isStart;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // roughly 90% of Vizio monitor resolution
            this.Width = 1728;
            this.Height = 972;
            this.SetDesktopLocation(20, 20);

            squareModel.Visible = false;
            squareModelAlive.Visible = false;

            myTimer.Enabled = false;
            myTimer.Interval = 550;

            stopToolStripMenuItem.Checked = true;
            middleSpeedToolStripMenuItem.Checked = true;

            cellSpacing = squareSize + squareSize / 4;

            // Create bitmap for grid rendering
            int bitmapWidth = squarePerLine * cellSpacing;
            int bitmapHeight = squarePerColumn * cellSpacing;
            gridBitmap = new Bitmap(bitmapWidth, bitmapHeight);

            // Create single PictureBox to display the bitmap
            gridDisplay = new PictureBox
            {
                Location = new Point(positionOffsetX, positionOffsetY),
                Size = new Size(bitmapWidth, bitmapHeight),
                Image = gridBitmap,
                SizeMode = PictureBoxSizeMode.Normal
            };
            gridDisplay.MouseClick += Grid_Click;
            this.Controls.Add(gridDisplay);

            this.Size = new Size(positionOffsetX + bitmapWidth + positionOffsetX, positionOffsetY + bitmapHeight + positionOffsetY + 50);

            InitializeGame();
            RenderGrid();

#if DEBUG
            // TestRleParser();  // Uncomment to test RLE parser
#endif
        }

        /// <summary>
        /// Initializes the game state, resetting all counters, UI elements, and the game grid.
        /// </summary>
        public void InitializeGame()
        {
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = " + TickNumber.ToString();
            colornumber = 0;

            // Set the initial color to the first color in Spectrum360
            squareModelAlive.BackColor = Color.FromArgb(
                ColorPalettes.Spectrum360[colornumber].red,
                ColorPalettes.Spectrum360[colornumber].green,
                ColorPalettes.Spectrum360[colornumber].blue);

            toolStripIterationsTextbox.BackColor = Color.White;
            AliveCount = 0;
            // ensure stability trackers are reset
            PreviousAliveCount = -1; // use -1 so first tick won't accidentally increment stability
            stableConsecutiveCount = 0;

            InitializeSquaresState(squaresState);

            // initialize cell color array to match initial states and count alive cells
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    if (squaresState[i, j])
                    {
                        AliveCount++;
                        cellColor[i, j] = squareModelAlive.BackColor;
                    }
                    else
                    {
                        cellColor[i, j] = squareModel.BackColor;
                    }
                }
            }

            // Update the display with actual count
            toolStripAliveCountBox.Text = AliveCount.ToString() + " cells alive.";
            this.toolStripAliveCountBox.AutoSize = true;  // Will expand
        }

        /// <summary>
        /// Initializes the state of all cells in the game grid.
        /// Randomly sets each cell to alive or dead state.
        /// </summary>
        /// <param name="squaresState">The 2D boolean array representing the grid state.</param>
        public static void InitializeSquaresState(bool[,] squaresState)
        {
            Random random = new Random();
            int rows = squaresState.GetLength(0);
            int cols = squaresState.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    squaresState[i, j] = random.Next(2) == 0;
                    //squaresState[i, j] = false;
                }
            }

            // Set up an R-pentomino
            //squaresState[2, 1] = true;
            //squaresState[3, 1] = true;
            //squaresState[1, 2] = true;
            //squaresState[2, 2] = true;
            //squaresState[2, 3] = true;
        }

        private void Grid_Click(object sender, MouseEventArgs e)
        {
            // Translate mouse coordinates to grid indices
            int i = e.X / cellSpacing;
            int j = e.Y / cellSpacing;

            if (i >= 0 && i < squarePerLine && j >= 0 && j < squarePerColumn)
            {
                // Toggle and keep AliveCount correct
                squaresState[i, j] = !squaresState[i, j];
                if (squaresState[i, j])
                {
                    AliveCount++;
                    // assign birth color (current generation color)
                    cellColor[i, j] = squareModelAlive.BackColor;
                }
                else
                {
                    AliveCount = Math.Max(0, AliveCount - 1); // avoid negative counts
                    // reset color when dead
                    cellColor[i, j] = squareModel.BackColor;
                }

                // Reset stability counter on manual change
                stableConsecutiveCount = 0;
                PreviousAliveCount = AliveCount;

                toolStripAliveCountBox.Text = AliveCount.ToString() + " cells alive.";
                RenderGrid();
            }
        }

        private List<(int i, int j)> changedCells = new List<(int, int)>();

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            changedCells.Clear();

            colornumber++;
            colornumber = colornumber % 360;
            squareModelAlive.BackColor = Color.FromArgb(ColorPalettes.Spectrum360[colornumber].red, ColorPalettes.Spectrum360[colornumber].green, ColorPalettes.Spectrum360[colornumber].blue);
            toolStripIterationsTextbox.BackColor = squareModelAlive.BackColor;
            System.Drawing.Color InverseColor = Color.FromArgb(255 - squareModelAlive.BackColor.R, 255 - squareModelAlive.BackColor.G, 255 - squareModelAlive.BackColor.B);
            toolStripIterationsTextbox.ForeColor = InverseColor;
            TickNumber++;
            toolStripIterationsTextbox.Text = "Tick = " + TickNumber.ToString();
            AliveCount = 0;

            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                // The following block of code determines the cell viability and subsequent state
                {
                    int NbVoisin = calculateNeighbors(i, j, squaresState);

                    if (squaresState[i, j]) // If the cell is alive
                    {
                        AliveCount++;
                        if (NbVoisin < ViabilityLowerBound || NbVoisin > ViabilityUpperBound)   // global bounds which can be changed for other values
                        {
                            squaresFutureState[i, j] = false; // Cell dies
                        }
                        else
                        {
                            squaresFutureState[i, j] = true; // Cell lives on to the next generation
                        }
                    }
                    else // If the cell is dead
                    {
                        if (NbVoisin == 3) // Standard condition for reproduction
                        {
                            squaresFutureState[i, j] = true; // Cell becomes alive
                        }
                        else
                        {
                            squaresFutureState[i, j] = false;
                        }
                    }

                    // If a cell is born this tick, record its birth color (current generation color)
                    if (squaresFutureState[i, j] && !squaresState[i, j])
                    {
                        cellColor[i, j] = squareModelAlive.BackColor;
                    }

                    if (squaresFutureState[i, j] != squaresState[i, j])
                    {
                        changedCells.Add((i, j));
                    }
                }
                // The preceding block of code determines the cell viability
            }
            toolStripAliveCountBox.Text = AliveCount.ToString() + " cells alive.";
            transferBoolArray(squaresPastState, squaresState);
            transferBoolArray(squaresState, squaresFutureState);

            // Stability: require the same alive count for N consecutive ticks
            if (AliveCount == PreviousAliveCount)
            {
                stableConsecutiveCount++;
            }
            else
            {
                stableConsecutiveCount = 0;
            }

            // Save for next tick comparison
            PreviousAliveCount = AliveCount;

            if (stableConsecutiveCount >= stableRequiredTicks)
            {
                stopToolStripMenuItem_Click(this, null);
                return;
            }

            RenderGridDirty(changedCells);
        }

        private void transferBoolArray(bool[,] TableauRecepteur, bool[,] TableauATransferer)
        {
            Buffer.BlockCopy(TableauATransferer, 0, TableauRecepteur, 0, TableauATransferer.Length * sizeof(bool));
        }

        private static int calculateNeighbors(int x, int y, bool[,] Tableau)
        {
            int NeighborsCount = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int neighborPosX = (x + i + squarePerLine) % squarePerLine;
                    int neighborPosY = (y + j + squarePerColumn) % squarePerColumn;

                    if (Tableau[neighborPosX, neighborPosY])
                        NeighborsCount++;
                }
            }

            if (Tableau[x, y])
                NeighborsCount--;

            return NeighborsCount;
        }

        private void slowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTimer.Interval = 1000;
            lentToolStripMenuItem.Checked = true;
            middleSpeedToolStripMenuItem.Checked = false;
            rapideToolStripMenuItem.Checked = false;
        }

        private void normalSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTimer.Interval = 550;
            lentToolStripMenuItem.Checked = false;
            middleSpeedToolStripMenuItem.Checked = true;
            rapideToolStripMenuItem.Checked = false;
        }

        private void quickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTimer.Interval = 200;  // Slow it down slightly to 200ms
            lentToolStripMenuItem.Checked = false;
            middleSpeedToolStripMenuItem.Checked = false;
            rapideToolStripMenuItem.Checked = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }
            InitializeGame();
            RenderGrid();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTimer.Enabled = true;
            resetToolStripMenuItem.Enabled = false;  // WAS: newToolStripMenuItem
            startToolStripMenuItem.Checked = true;    // WAS: runToolStripMenuItem
            stopToolStripMenuItem.Checked = false;

            // Update status display
            toolStripTextBoxStatus.Text = "Running";
            toolStripTextBoxStatus.ForeColor = Color.Green;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTimer.Enabled = false;
            resetToolStripMenuItem.Enabled = true;   // WAS: newToolStripMenuItem
            startToolStripMenuItem.Checked = false;   // WAS: runToolStripMenuItem
            stopToolStripMenuItem.Checked = true;

            // Update status display
            toolStripTextBoxStatus.Text = "Stopped";
            toolStripTextBoxStatus.ForeColor = Color.Red;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuEtatVitesse_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void RenderGrid()
        {
            using (Graphics g = Graphics.FromImage(gridBitmap))
            {
                Brush deadBrush = new SolidBrush(squareModel.BackColor);
                Dictionary<Color, Brush> aliveBrushes = new Dictionary<Color, Brush>();

                for (int i = 0; i < squarePerLine; i++)
                {
                    for (int j = 0; j < squarePerColumn; j++)
                    {
                        Brush brush;
                        if (squaresState[i, j])
                        {
                            Color aliveColor = cellColor[i, j];
                            if (!aliveBrushes.TryGetValue(aliveColor, out brush))
                            {
                                brush = new SolidBrush(aliveColor);
                                aliveBrushes[aliveColor] = brush;
                            }
                        }
                        else
                        {
                            brush = deadBrush;
                        }

                        g.FillRectangle(brush, i * cellSpacing, j * cellSpacing, squareSize, squareSize);
                    }
                }

                deadBrush.Dispose();
                foreach (var b in aliveBrushes.Values)
                    b.Dispose();
            }
            gridDisplay.Invalidate();
        }

        private void RenderGridDirty(List<(int i, int j)> changedCells)
        {
            using (Graphics g = Graphics.FromImage(gridBitmap))
            {
                Brush deadBrush = new SolidBrush(squareModel.BackColor);
                Dictionary<Color, Brush> aliveBrushes = new Dictionary<Color, Brush>();

                foreach (var (i, j) in changedCells)
                {
                    Brush brush;
                    if (squaresState[i, j])
                    {
                        Color aliveColor = cellColor[i, j];
                        if (!aliveBrushes.TryGetValue(aliveColor, out brush))
                        {
                            brush = new SolidBrush(aliveColor);
                            aliveBrushes[aliveColor] = brush;
                        }
                    }
                    else
                    {
                        brush = deadBrush;
                    }

                    g.FillRectangle(brush, i * cellSpacing, j * cellSpacing, squareSize, squareSize);
                }

                deadBrush.Dispose();
                foreach (var b in aliveBrushes.Values)
                    b.Dispose();
            }
            gridDisplay.Invalidate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            randomToolStripMenuItem.Checked = true;

            // Reinitialize with random pattern
            InitializeGame();
            RenderGrid();
        }

        private void exportPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "RLE Pattern Files (*.rle)|*.rle|All Files (*.*)|*.*";
                sfd.DefaultExt = "rle";
                sfd.Title = "Export Pattern";
                sfd.FileName = "pattern.rle";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create pattern from current grid state
                        var pattern = new GameOfLife.Pattern(squaresState)
                        {
                            Name = "Exported Pattern",
                            Description = $"Exported at tick {TickNumber} with {AliveCount} alive cells"
                        };

                        // Write to file
                        string rleContent = GameOfLife.RleParser.Write(pattern);
                        System.IO.File.WriteAllText(sfd.FileName, rleContent);

                        MessageBox.Show($"Pattern exported successfully to:\n{sfd.FileName}",
                                       "Export Successful",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting pattern:\n{ex.Message}",
                                       "Export Error",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void importPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "RLE Pattern Files (*.rle)|*.rle|All Files (*.*)|*.*";
                ofd.Title = "Import Pattern";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read and parse the RLE file
                        string rleContent = System.IO.File.ReadAllText(ofd.FileName);
                        GameOfLife.Pattern pattern = GameOfLife.RleParser.Parse(rleContent);

                        // Show preview dialog
                        using (var previewDialog = new PatternPreviewDialog(pattern))
                        {
                            if (previewDialog.ShowDialog() == DialogResult.OK)
                            {
                                // User clicked Load - apply pattern to grid
                                LoadPatternToGrid(pattern);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error importing pattern:\n{ex.Message}",
                                       "Import Error",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadPatternToGrid(GameOfLife.Pattern pattern)
        {
            // Clear the grid
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Center the pattern on the grid
            int startX = (squarePerLine - pattern.Width) / 2;
            int startY = (squarePerColumn - pattern.Height) / 2;

            // Load pattern cells
            AliveCount = 0;
            for (int x = 0; x < pattern.Width && startX + x < squarePerLine; x++)
            {
                for (int y = 0; y < pattern.Height && startY + y < squarePerColumn; y++)
                {
                    if (pattern.Cells[x, y])
                    {
                        squaresState[startX + x, startY + y] = true;
                        cellColor[startX + x, startY + y] = squareModelAlive.BackColor;
                        AliveCount++;
                    }
                }
            }

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " cells alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the loaded pattern
            RenderGrid();

            MessageBox.Show($"Pattern '{pattern.Name}' loaded successfully!\n{AliveCount} cells alive.",
                           "Import Successful",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }

        private void TestRleParser()
        {
            // Simple glider pattern in RLE format
            string gliderRle = @"#N Glider
#C A small spaceship
x = 3, y = 3, rule = B3/S23
bob$2bo$3o!";

            try
            {
                // Test parsing
                GameOfLife.Pattern pattern = GameOfLife.RleParser.Parse(gliderRle);

                MessageBox.Show($"Parse successful!\n" +
                               $"Name: {pattern.Name}\n" +
                               $"Description: {pattern.Description}\n" +
                               $"Size: {pattern.Width}x{pattern.Height}\n" +
                               $"Rule: {pattern.Rule}",
                               "RLE Parser Test");

                // Test writing
                string written = GameOfLife.RleParser.Write(pattern);
                MessageBox.Show($"Write successful!\n\n{written}", "RLE Writer Test");

                // Verify roundtrip
                GameOfLife.Pattern parsed2 = GameOfLife.RleParser.Parse(written);
                bool cellsMatch = true;
                for (int x = 0; x < pattern.Width; x++)
                    for (int y = 0; y < pattern.Height; y++)
                        if (pattern.Cells[x, y] != parsed2.Cells[x, y])
                            cellsMatch = false;

                MessageBox.Show(cellsMatch ? "Roundtrip test PASSED!" : "Roundtrip test FAILED!",
                               "Roundtrip Test");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Test FAILED: {ex.Message}\n\nStack trace:\n{ex.StackTrace}",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }
    }
}
