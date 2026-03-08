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
    /// Defines the coloring mode for cells in the simulation.
    /// </summary>
    public enum ColorMode
    {
        /// <summary>
        /// Cells are colored based on the generation they were born in, and retain that color for their lifetime.
        /// </summary>
        BirthGeneration,

        /// <summary>
        /// Cells start blue and age through the color spectrum as they survive.
        /// </summary>
        CellAging
    }

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

        /// <summary>
        /// Multiplier for cell aging color progression. Higher values = faster color changes.
        /// 1 = normal (1 color per generation), 2 = 2x speed, 3 = 3x speed, etc.
        /// </summary>
        private const int cellAgingColorFactor = 3;  // Adjust this to change aging speed!

        // per-cell color to preserve birth color (age)
        private Color[,] cellColor = new Color[squarePerLine, squarePerColumn];

        // Coloring mode fields (for future extensibility)
        private static ColorMode currentColorMode = ColorMode.BirthGeneration;

        private static int[,] cellAge = new int[squarePerLine, squarePerColumn];

        // Mouse drag editing fields
        private bool isDragging = false;

        private bool dragPaintMode = true; // true = paint alive, false = erase
        private Point currentMouseCell = new Point(-1, -1); // Current cell under mouse
        private bool mouseOverGrid = false; // Is mouse over the grid

        /// <summary>
        /// Population density percentage (0-100). Controls the proportion of alive cells.
        /// Adjustable via mouse wheel in 5% increments. Default is 50%.
        /// </summary>
        private int populationDensity = 50;

        /// <summary>
        /// Tracks whether to show the density overlay (true when mouse wheel was recently used).
        /// </summary>
        private bool showDensityOverlay = false;

        /// <summary>
        /// Timer to hide the density overlay after 5 seconds of inactivity.
        /// </summary>
        private System.Windows.Forms.Timer densityOverlayTimer;

        // Renderer for grid visualization
        private GridRenderer gridRenderer;

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

            // Set up color mode combo box
            colorModeComboBox.Items.Add("Constant Color from Birth");
            colorModeComboBox.Items.Add("Cell Changes Color as Aging");
            colorModeComboBox.SelectedIndex = 0; // Default to Constant Color from Birth
            colorModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList; // Prevent typing
            colorModeComboBox.Width = 400; // Wide enough for longer descriptions

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

            gridDisplay.MouseDown += Grid_MouseDown;
            gridDisplay.MouseMove += Grid_MouseMove;
            gridDisplay.MouseUp += Grid_MouseUp;
            gridDisplay.MouseEnter += Grid_MouseEnter;
            gridDisplay.MouseLeave += Grid_MouseLeave;
            gridDisplay.MouseWheel += Grid_MouseWheel;
            gridDisplay.Paint += Grid_Paint; // Add paint handler for cursor overlay
            this.Controls.Add(gridDisplay);

            this.Size = new Size(positionOffsetX + bitmapWidth + positionOffsetX, positionOffsetY + bitmapHeight + positionOffsetY + 50);

            InitializeGame();

            // Initialize the grid renderer
            gridRenderer = new GridRenderer(gridBitmap, gridDisplay, squareModel, squarePerLine, squarePerColumn, squareSize, cellSpacing);

            // Initialize density overlay timer (5 seconds)
            densityOverlayTimer = new System.Windows.Forms.Timer();
            densityOverlayTimer.Interval = 5000; // 5 seconds
            densityOverlayTimer.Tick += (s, ev) =>
            {
                showDensityOverlay = false;
                densityOverlayTimer.Stop();
                gridDisplay.Invalidate(); // Redraw to hide overlay
            };

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
            // colornumber =   0;  // this sets the first color to index 0 of the palette (Red) on each new game
            colornumber = 241;  // this sets the first color to index 241 in (0,0,255) in the palette (Blue)

            // Set the initial color using the colornumber as the index in Spectrum360
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

                        // Set color based on current mode
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
                        }
                        else // CellAging mode
                        {
                            cellAge[i, j] = 0; // All initial cells start at age 0
                            int colorIndex = 240; // Blue (0,0,255)
                            cellColor[i, j] = Color.FromArgb(
                                ColorPalettes.Spectrum360[colorIndex].red,
                                ColorPalettes.Spectrum360[colorIndex].green,
                                ColorPalettes.Spectrum360[colorIndex].blue);
                        }
                    }
                    else
                    {
                        cellColor[i, j] = squareModel.BackColor;
                        cellAge[i, j] = 0;
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

        private List<(int i, int j)> changedCells = new List<(int, int)>();

        private void tmrClock_Tick(object sender, EventArgs e)
        {
            changedCells.Clear();

            UpdateGenerationColor();

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
                    // Handle cell birth and aging based on color mode
                    bool cellStateChanged = false;
                    bool cellAged = false;

                    if (squaresFutureState[i, j] && !squaresState[i, j])
                    {
                        // Cell is born this tick
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
                        }
                        else // CellAging mode
                        {
                            cellAge[i, j] = 0; // Start at age 0
                            int colorIndex = 240; // Blue (0,0,255)
                            cellColor[i, j] = Color.FromArgb(
                                ColorPalettes.Spectrum360[colorIndex].red,
                                ColorPalettes.Spectrum360[colorIndex].green,
                                ColorPalettes.Spectrum360[colorIndex].blue);
                        }
                        cellStateChanged = true;
                    }
                    else if (squaresFutureState[i, j] && squaresState[i, j])
                    {
                        // Cell survives - age it if in CellAging mode
                        if (currentColorMode == ColorMode.CellAging)
                        {
                            cellAge[i, j]++;
                            // Apply aging factor and allow continuous cycling through palette
                            int ageDisplay = cellAge[i, j] * cellAgingColorFactor;
                            int colorIndex = (240 + ageDisplay) % ColorPalettes.Spectrum360.Length;
                            cellColor[i, j] = Color.FromArgb(
                                ColorPalettes.Spectrum360[colorIndex].red,
                                ColorPalettes.Spectrum360[colorIndex].green,
                                ColorPalettes.Spectrum360[colorIndex].blue);
                            cellAged = true;
                        }
                    }
                    else if (!squaresFutureState[i, j] && squaresState[i, j])
                    {
                        // Cell dies
                        cellStateChanged = true;
                    }

                    // Add to changed cells if state changed OR cell aged
                    if (cellStateChanged || cellAged)
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
            gridRenderer.RenderGrid(squaresState, cellColor);
        }

        private void RenderGridDirty(List<(int i, int j)> changedCells)
        {
            gridRenderer.RenderGridDirty(squaresState, cellColor, changedCells);
        }

        /// <summary>
        /// Updates the generation color based on current color mode.
        /// </summary>
        private void UpdateGenerationColor()
        {
            if (currentColorMode == ColorMode.BirthGeneration)
            {
                colornumber++;
                colornumber = colornumber % ColorPalettes.Spectrum360.Length;
                squareModelAlive.BackColor = Color.FromArgb(
                    ColorPalettes.Spectrum360[colornumber].red,
                    ColorPalettes.Spectrum360[colornumber].green,
                    ColorPalettes.Spectrum360[colornumber].blue);

                // Update tick counter color only in BirthGeneration mode
                toolStripIterationsTextbox.BackColor = squareModelAlive.BackColor;
            }
            // In CellAging mode, we don't update the global color or tick counter
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

        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            // Start dragging
            isDragging = true;

            // Determine paint or erase mode based on button
            if (e.Button == MouseButtons.Left)
                dragPaintMode = true;  // Left = paint alive
            else if (e.Button == MouseButtons.Right)
                dragPaintMode = false; // Right = erase
            else
                return; // Ignore other buttons

            // Paint/erase the initial cell
            PaintCell(e.X, e.Y);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            // Update current cell position
            int cellI = e.X / cellSpacing;
            int cellJ = e.Y / cellSpacing;

            if (cellI != currentMouseCell.X || cellJ != currentMouseCell.Y)
            {
                currentMouseCell = new Point(cellI, cellJ);
                gridDisplay.Invalidate(); // Redraw to show cursor
            }

            // Only process if we're dragging
            if (!isDragging)
                return;

            // Paint/erase cells as we drag to
            PaintCell(e.X, e.Y);
        }

        private void Grid_MouseUp(object sender, MouseEventArgs e)
        {
            // Stop dragging
            isDragging = false;
        }

        private void Grid_MouseWheel(object sender, MouseEventArgs e)
        {
            // Adjust density in 5% increments
            if (e.Delta > 0)
            {
                // Scroll up = increase density
                populationDensity = Math.Min(100, populationDensity + 5);
            }
            else if (e.Delta < 0)
            {
                // Scroll down = decrease density
                populationDensity = Math.Max(0, populationDensity - 5);
            }

            // Show density overlay and restart timer
            showDensityOverlay = true;
            densityOverlayTimer.Stop(); // Reset timer
            densityOverlayTimer.Start(); // Start 5-second countdown
            gridDisplay.Invalidate(); // Trigger redraw to show density

            // Apply density to grid
            ApplyDensityToGrid();
        }

        /// <summary>
        /// Adjusts the grid to match the target population density by randomly adding or removing cells.
        /// </summary>
        private void ApplyDensityToGrid()
        {
            int totalCells = squarePerLine * squarePerColumn;
            int targetAliveCount = (int)Math.Round(totalCells * populationDensity / 100.0);
            int currentAliveCount = AliveCount;

            Random random = new Random();
            List<(int, int)> changedCells = new List<(int, int)>();

            if (targetAliveCount > currentAliveCount)
            {
                // Need to add cells
                int cellsToAdd = targetAliveCount - currentAliveCount;
                int attempts = 0;
                int maxAttempts = cellsToAdd * 150; // Higher multiplier for sparse grids

                while (cellsToAdd > 0 && attempts < maxAttempts)
                {
                    int i = random.Next(squarePerLine);
                    int j = random.Next(squarePerColumn);

                    if (!squaresState[i, j]) // Cell is dead
                    {
                        squaresState[i, j] = true;
                        AliveCount++;

                        // Set color based on current mode
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                        }
                        else // CellAging mode
                        {
                            cellAge[i, j] = 0;
                            int colorIndex = 240; // Blue
                            cellColor[i, j] = Color.FromArgb(
                                ColorPalettes.Spectrum360[colorIndex].red,
                                ColorPalettes.Spectrum360[colorIndex].green,
                                ColorPalettes.Spectrum360[colorIndex].blue);
                        }

                        changedCells.Add((i, j));
                        cellsToAdd--;
                    }
                    attempts++;
                }
            }
            else if (targetAliveCount < currentAliveCount)
            {
                // Need to remove cells
                int cellsToRemove = currentAliveCount - targetAliveCount;
                int attempts = 0;
                int maxAttempts = cellsToRemove * 150; // Higher multiplier for sparse grids

                while (cellsToRemove > 0 && attempts < maxAttempts)
                {
                    int i = random.Next(squarePerLine);
                    int j = random.Next(squarePerColumn);

                    if (squaresState[i, j]) // Cell is alive
                    {
                        squaresState[i, j] = false;
                        AliveCount--;
                        cellColor[i, j] = squareModel.BackColor;
                        changedCells.Add((i, j));
                        cellsToRemove--;
                    }
                    attempts++;
                }
            }

            // Update UI and render
            if (changedCells.Count > 0)
            {
                toolStripAliveCountBox.Text = AliveCount.ToString() + " cells alive.";
                RenderGridDirty(changedCells);
            }
        }

        private void Grid_MouseEnter(object sender, EventArgs e)
        {
            mouseOverGrid = true;
            gridDisplay.Invalidate();
        }

        private void Grid_MouseLeave(object sender, EventArgs e)
        {
            mouseOverGrid = false;
            currentMouseCell = new Point(-1, -1);
            gridDisplay.Invalidate();
        }

        private void PaintCell(int mouseX, int mouseY)
        {
            // Translate mouse coordinates to grid indices (center of brush)
            int centerI = mouseX / cellSpacing;
            int centerJ = mouseY / cellSpacing;

            // Check center bounds
            if (centerI < 0 || centerI >= squarePerLine || centerJ < 0 || centerJ >= squarePerColumn)
                return;

            // Track changed cells for dirty rendering
            List<(int, int)> cellsToUpdate = new List<(int, int)>();

            if (dragPaintMode)
            {
                // FINE-POINT SHARPIE: Paint only single cell
                if (PaintSingleCell(centerI, centerJ, true))
                    cellsToUpdate.Add((centerI, centerJ));
            }
            else
            {
                // BROAD ERASER: Erase 5×5 area
                for (int di = -2; di <= 2; di++)
                {
                    for (int dj = -2; dj <= 2; dj++)
                    {
                        int i = centerI + di;
                        int j = centerJ + dj;

                        // Check bounds for each cell in the eraser area
                        if (i >= 0 && i < squarePerLine && j >= 0 && j < squarePerColumn)
                        {
                            if (PaintSingleCell(i, j, false))
                                cellsToUpdate.Add((i, j));
                        }
                    }
                }
            }

            // Only update display if cells actually changed
            if (cellsToUpdate.Count > 0)
            {
                toolStripAliveCountBox.Text = AliveCount.ToString() + " cells alive.";
                RenderGridDirty(cellsToUpdate);  // Only redraw changed cells!
            }
        }

        private bool PaintSingleCell(int i, int j, bool shouldBeAlive)
        {
            // Get current state
            bool wasAlive = squaresState[i, j];

            // Only update if state changes
            if (wasAlive == shouldBeAlive)
                return false;  // No change

            // Update cell state
            squaresState[i, j] = shouldBeAlive;

            // Update color and count
            if (shouldBeAlive)
            {
                AliveCount++;

                // Set color based on current mode
                if (currentColorMode == ColorMode.BirthGeneration)
                {
                    cellColor[i, j] = squareModelAlive.BackColor;
                }
                else // CellAging mode
                {
                    cellAge[i, j] = 0; // New manually painted cell starts at age 0
                    int colorIndex = 240; // Blue (0,0,255)
                    cellColor[i, j] = Color.FromArgb(
                        ColorPalettes.Spectrum360[colorIndex].red,
                        ColorPalettes.Spectrum360[colorIndex].green,
                        ColorPalettes.Spectrum360[colorIndex].blue);
                }
            }
            else
            {
                AliveCount = Math.Max(0, AliveCount - 1);
                cellColor[i, j] = squareModel.BackColor;
            }

            // Reset stability counter on manual change
            stableConsecutiveCount = 0;
            PreviousAliveCount = AliveCount;

            return true;  // Cell changed
        }

        private void Grid_Paint(object sender, PaintEventArgs e)
        {
            // Show density overlay if mouse wheel was used
            if (showDensityOverlay && mouseOverGrid)
            {
                string densityText = $"Density: {populationDensity}%";
                using (Font font = new Font("Segoe UI", 16, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.Black))
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(220, 255, 200, 0))) // Semi-transparent orange
                {
                    SizeF textSize = e.Graphics.MeasureString(densityText, font);

                    // Position near mouse cursor (offset to avoid covering cells)
                    Point mousePos = gridDisplay.PointToClient(Cursor.Position);
                    int overlayX = mousePos.X + 20;
                    int overlayY = mousePos.Y - 30;

                    // Draw background box
                    e.Graphics.FillRectangle(bgBrush, overlayX, overlayY, textSize.Width + 10, textSize.Height + 5);

                    // Draw text
                    e.Graphics.DrawString(densityText, font, textBrush, overlayX + 5, overlayY + 2);
                }
            }

            // Only show cursor when mouse is over grid and we have a valid cell position
            if (!mouseOverGrid || currentMouseCell.X < 0 || currentMouseCell.Y < 0)
                return;

            int cellI = currentMouseCell.X;
            int cellJ = currentMouseCell.Y;

            // Check bounds
            if (cellI < 0 || cellI >= squarePerLine || cellJ < 0 || cellJ >= squarePerColumn)
                return;

            // Draw cursor overlay based on current mode
            using (Pen cursorPen = new Pen(Color.Black, 2))
            {
                if (isDragging && !dragPaintMode) // Show eraser cursor right-click dragging
                {
                    // Calculate 5×5 eraser area bounds (centered on current cell)
                    int eraserRadius = 2; // -2 to +2 = 5 cells
                    int startI = Math.Max(0, cellI - eraserRadius);
                    int startJ = Math.Max(0, cellJ - eraserRadius);
                    int endI = Math.Min(squarePerLine - 1, cellI + eraserRadius);
                    int endJ = Math.Min(squarePerColumn - 1, cellJ + eraserRadius);

                    // Calculate pixel coordinates
                    int x = startI * cellSpacing - 1;
                    int y = startJ * cellSpacing - 1;
                    int width = (endI - startI + 1) * cellSpacing + 2;
                    int height = (endJ - startJ + 1) * cellSpacing + 2;

                    // Draw rounded rectangle
                    int cornerRadius = 8;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddArc(x, y, cornerRadius, cornerRadius, 180, 90);
                        path.AddArc(x + width - cornerRadius, y, cornerRadius, cornerRadius, 270, 90);
                        path.AddArc(x + width - cornerRadius, y + height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                        path.AddArc(x, y + height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                        path.CloseFigure();

                        e.Graphics.DrawPath(cursorPen, path);
                    }
                }
            }
        }

        private void clearGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear all cells to dead
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Reset counters
            TickNumber = 0;
            AliveCount = 0;
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Update UI
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = "0 cells alive.";
            toolStripIterationsTextbox.BackColor = Color.White;

            // Render empty grid
            RenderGrid();
        }

        private void colorModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Switch the color mode based on selection
            ColorMode newMode = colorModeComboBox.SelectedIndex == 0
                ? ColorMode.BirthGeneration
                : ColorMode.CellAging;

            if (newMode != currentColorMode)
            {
                currentColorMode = newMode;

                // Update tick counter appearance based on mode
                if (currentColorMode == ColorMode.CellAging)
                {
                    // Reset to white background with black text in CellAging mode
                    toolStripIterationsTextbox.BackColor = Color.White;
                    toolStripIterationsTextbox.ForeColor = Color.Black;
                }
                else
                {
                    // Restore generation color in BirthGeneration mode
                    toolStripIterationsTextbox.BackColor = squareModelAlive.BackColor;
                    System.Drawing.Color InverseColor = Color.FromArgb(
                        255 - squareModelAlive.BackColor.R,
                        255 - squareModelAlive.BackColor.G,
                        255 - squareModelAlive.BackColor.B);
                    toolStripIterationsTextbox.ForeColor = InverseColor;
                }

                // Re-color all existing alive cells based on new mode
                RecolorCellsForMode();

                // Redraw the grid
                RenderGrid();
            }
        }

        /// <summary>
        /// Re-colors all existing alive cells based on the current color mode.
        /// </summary>
        private void RecolorCellsForMode()
        {
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    if (squaresState[i, j]) // If cell is alive
                    {
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            // Use current generation color
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
                        }
                        else // CellAging mode
                        {
                            // Apply aging factor for consistent coloring
                            int ageDisplay = cellAge[i, j] * cellAgingColorFactor;
                            int colorIndex = (240 + ageDisplay) % ColorPalettes.Spectrum360.Length;
                            cellColor[i, j] = Color.FromArgb(
                                ColorPalettes.Spectrum360[colorIndex].red,
                                ColorPalettes.Spectrum360[colorIndex].green,
                                ColorPalettes.Spectrum360[colorIndex].blue);
                        }
                    }
                }
            }
        }
    }
}
