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
    /// Defines the interaction mode for the grid.
    /// </summary>
    public enum InteractionMode
    {
        /// <summary>
        /// Normal drawing mode - paint and erase cells with mouse.
        /// </summary>
        Drawing,

        /// <summary>
        /// Selection mode - select a rectangular region to tile across the grid.
        /// </summary>
        TilingSelection
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

        // Tiling selection mode fields
        private InteractionMode currentMode = InteractionMode.Drawing;

        private bool isSelecting = false;
        private Point selectionStart = Point.Empty;
        private Point selectionEnd = Point.Empty;
        private Rectangle selectionRect = Rectangle.Empty;
        private bool isCtrlKeyHeld = false; // Track Ctrl key state for temporary selection mode
        private bool isTemporarySelectionMode = false; // True for Ctrl+drag, false for T-key persistent mode

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

        // Clipboard fields for copy/paste with transforms
        private bool[,] clipboard = null;

        private Color[,] clipboardColors = null;
        private int clipboardWidth = 0;
        private int clipboardHeight = 0;
        private bool pasteMode = false;
        private Point pastePreviewCell = new Point(-1, -1);

        // Renderer for grid visualization
        private GridRenderer gridRenderer;

        /// <summary>
        /// Form used for rendering the entire grid.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
            this.KeyPreview = true; // Ensure form receives key events before controls
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
            else if (e.KeyCode == Keys.C && e.Control && !e.Shift && !e.Alt)
            {
                // Copy selection (Ctrl+C)
                CopySelection();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.V && e.Control && !e.Shift && !e.Alt)
            {
                // Paste (Ctrl+V)
                if (clipboard != null)
                {
                    EnterPasteMode();
                }
                e.Handled = true;
            }
            else if (pasteMode && e.KeyCode == Keys.H && !e.Control && !e.Shift && !e.Alt)
            {
                // Flip horizontal in paste mode
                FlipClipboardHorizontal();
                gridDisplay.Invalidate();
            }
            else if (pasteMode && e.KeyCode == Keys.V && !e.Control && !e.Shift && !e.Alt)
            {
                // Flip vertical in paste mode
                FlipClipboardVertical();
                gridDisplay.Invalidate();
            }
            else if (pasteMode && e.KeyCode == Keys.R && !e.Control && !e.Shift && !e.Alt)
            {
                // Rotate 90° CW in paste mode
                RotateClipboard90CW();
                gridDisplay.Invalidate();
            }
            else if (pasteMode && e.KeyCode == Keys.R && !e.Control && e.Shift && !e.Alt)
            {
                // Rotate 90° CCW in paste mode (Shift+R)
                RotateClipboard90CCW();
                gridDisplay.Invalidate();
            }
            else if (e.KeyCode == Keys.T && !e.Control && !e.Shift && !e.Alt)
            {
                // Toggle selection mode with T key
                tileSelectionToolStripMenuItem_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                // Exit paste mode or selection mode with Escape
                if (pasteMode)
                {
                    ExitPasteMode();
                }
                else if (currentMode == InteractionMode.TilingSelection || !selectionRect.IsEmpty)
                {
                    ExitSelectionMode();
                }
            }
            else if (e.KeyCode == Keys.Enter && !selectionRect.IsEmpty)
            {
                // Apply tiling with Enter key (works for both modes)
                ApplyTiling();
            }
            else if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                // Ctrl key pressed - update state and cursor
                if (!isCtrlKeyHeld && mouseOverGrid)
                {
                    isCtrlKeyHeld = true;
                    UpdateCursorForCtrlKey();
                    UpdateModeIndicatorForCtrl();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                // Ctrl key released - update state and cursor
                isCtrlKeyHeld = false;
                UpdateCursorForCtrlKey();
                RestoreModeIndicator();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form will start at design-time size and be resizable
            // Grid panel will show scrollbars when needed

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
                SizeMode = PictureBoxSizeMode.Normal,
                TabStop = true  // Make grid focusable so it can receive keyboard events
            };

            gridDisplay.MouseDown += Grid_MouseDown;
            gridDisplay.MouseMove += Grid_MouseMove;
            gridDisplay.MouseUp += Grid_MouseUp;
            gridDisplay.MouseEnter += Grid_MouseEnter;
            gridDisplay.MouseLeave += Grid_MouseLeave;
            gridDisplay.MouseWheel += Grid_MouseWheel;
            gridDisplay.Paint += Grid_Paint; // Add paint handler for cursor overlay
            gridDisplay.Click += (s, e) => gridDisplay.Focus(); // Focus grid when clicked
            gridPanel.Controls.Add(gridDisplay);  // Add to panel instead of form

            // Don't manually size the form - let it use the design-time size
            // The gridPanel will handle scrolling when the grid is larger than the visible area

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

            // Initialize Edit menu state
            UpdateEditMenuState();

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
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
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
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
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

                // Set foreground to complementary color for readability
                Color complementaryColor = Color.FromArgb(
                    255 - squareModelAlive.BackColor.R,
                    255 - squareModelAlive.BackColor.G,
                    255 - squareModelAlive.BackColor.B);
                toolStripIterationsTextbox.ForeColor = complementaryColor;
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
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the loaded pattern
            RenderGrid();

            MessageBox.Show($"Pattern '{pattern.Name}' loaded successfully!\n{AliveCount} cells alive.",
                           "Import Successful",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }

        /// <summary>
        /// Loads a preset pattern from the pattern library.
        /// </summary>
        private void LoadPresetPattern(string patternName)
        {
            try
            {
                var patterns = GameOfLife.PatternLibrary.GetAllPatterns();
                if (!patterns.ContainsKey(patternName)) return;

                string rleContent = patterns[patternName];
                GameOfLife.Pattern pattern = GameOfLife.RleParser.Parse(rleContent);
                LoadPatternToGrid(pattern);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading preset pattern:\n{ex.Message}", "Load Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gliderToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Glider");

        private void lwssToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Lightweight Spaceship (LWSS)");

        private void mwssToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Middleweight Spaceship (MWSS)");

        private void hwssToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Heavyweight Spaceship (HWSS)");

        private void blinkerToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Blinker");

        private void toadToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Toad");

        private void beaconToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Beacon");

        private void pulsarToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Pulsar");

        private void pentadecathlonToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Pentadecathlon");

        private void rPentominoToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("R-Pentomino");

        private void acornToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Acorn");

        private void diehardToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Diehard");

        private void piHeptominoToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Pi Heptomino");

        private void gosperGliderGunToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Gosper Glider Gun");

        private void eater1ToolStripMenuItem_Click(object sender, EventArgs e) => LoadPresetPattern("Eater 1");

        /// <summary>
        /// Handles the Tile Selection menu item click to enter selection mode.
        /// </summary>
        private void tileSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMode == InteractionMode.Drawing)
            {
                EnterSelectionMode();
            }
            else
            {
                ExitSelectionMode();
            }
        }

        /// <summary>
        /// Enters tiling selection mode.
        /// </summary>
        private void EnterSelectionMode()
        {
            currentMode = InteractionMode.TilingSelection;
            isSelecting = false;
            selectionStart = Point.Empty;
            selectionEnd = Point.Empty;
            selectionRect = Rectangle.Empty;
            isTemporarySelectionMode = false; // T-key is persistent mode

            // Update UI
            toolStripModeIndicator.Text = "Mode: Select Region (T)";
            toolStripModeIndicator.BackColor = Color.LightSkyBlue;
            tileSelectionToolStripMenuItem.Text = "Exit Selection Mode (Esc)";

            // Change cursor
            gridDisplay.Cursor = Cursors.Cross;

            gridDisplay.Invalidate();
        }

        /// <summary>
        /// Exits tiling selection mode and returns to drawing mode.
        /// </summary>
        private void ExitSelectionMode()
        {
            currentMode = InteractionMode.Drawing;
            isSelecting = false;
            selectionStart = Point.Empty;
            selectionEnd = Point.Empty;
            selectionRect = Rectangle.Empty;
            isTemporarySelectionMode = false;

            // Update UI
            toolStripModeIndicator.Text = "Mode: Drawing";
            toolStripModeIndicator.BackColor = Color.LightGreen;
            tileSelectionToolStripMenuItem.Text = "Tile Selection (Ctrl+Drag or T)";

            // Reset cursor (respecting Ctrl key state)
            UpdateCursorForCtrlKey();

            gridDisplay.Invalidate();
        }

        /// <summary>
        /// Temporarily enters selection mode for Ctrl+drag operation.
        /// </summary>
        private void EnterTemporarySelectionMode()
        {
            currentMode = InteractionMode.TilingSelection;
            isTemporarySelectionMode = true; // Ctrl+drag is temporary mode

            // Update UI to show temporary selection mode
            toolStripModeIndicator.Text = "Mode: Select Region (Ctrl)";
            toolStripModeIndicator.BackColor = Color.LightSkyBlue;

            // Cursor already set to crosshair by Ctrl key
        }

        /// <summary>
        /// Updates cursor based on Ctrl key state.
        /// </summary>
        private void UpdateCursorForCtrlKey()
        {
            if (!mouseOverGrid)
                return;

            if (currentMode == InteractionMode.TilingSelection)
            {
                gridDisplay.Cursor = Cursors.Cross;
            }
            else if (isCtrlKeyHeld)
            {
                gridDisplay.Cursor = Cursors.Cross; // Show selection cursor when Ctrl is held
            }
            else
            {
                gridDisplay.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Updates mode indicator to show Ctrl+drag hint.
        /// </summary>
        private void UpdateModeIndicatorForCtrl()
        {
            if (currentMode == InteractionMode.Drawing && mouseOverGrid)
            {
                toolStripModeIndicator.Text = "Ctrl+Drag to Select";
                toolStripModeIndicator.BackColor = Color.LightCyan;
            }
        }

        /// <summary>
        /// Restores mode indicator to normal state.
        /// </summary>
        private void RestoreModeIndicator()
        {
            if (currentMode == InteractionMode.Drawing)
            {
                toolStripModeIndicator.Text = "Mode: Drawing";
                toolStripModeIndicator.BackColor = Color.LightGreen;
            }
        }

        /// <summary>
        /// Applies the tiling of the selected region across the entire grid.
        /// The selection defines the tile unit (pattern + spacing) which is repeated seamlessly.
        /// </summary>
        private void ApplyTiling()
        {
            if (selectionRect.IsEmpty || selectionRect.Width == 0 || selectionRect.Height == 0)
            {
                MessageBox.Show("No valid region selected for tiling.", "Tiling Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Convert pixel coordinates to cell coordinates
            int startCellX = selectionRect.X / cellSpacing;
            int startCellY = selectionRect.Y / cellSpacing;
            int endCellX = (selectionRect.X + selectionRect.Width) / cellSpacing;
            int endCellY = (selectionRect.Y + selectionRect.Height) / cellSpacing;

            // Ensure bounds
            startCellX = Math.Max(0, startCellX);
            startCellY = Math.Max(0, startCellY);
            endCellX = Math.Min(squarePerLine, endCellX);
            endCellY = Math.Min(squarePerColumn, endCellY);

            // Calculate pattern dimensions (tile unit size)
            // Note: No +1 here because endCell is already one past the last cell
            int patternWidth = endCellX - startCellX;
            int patternHeight = endCellY - startCellY;

            if (patternWidth <= 0 || patternHeight <= 0)
            {
                MessageBox.Show("Selected region is too small.", "Tiling Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Capture the tile unit (pattern + spacing + colors)
            bool[,] pattern = new bool[patternWidth, patternHeight];
            Color[,] patternColors = new Color[patternWidth, patternHeight];

            for (int i = 0; i < patternWidth; i++)
            {
                for (int j = 0; j < patternHeight; j++)
                {
                    pattern[i, j] = squaresState[startCellX + i, startCellY + j];
                    patternColors[i, j] = cellColor[startCellX + i, startCellY + j];
                }
            }

            // Clear the grid
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellAge[i, j] = 0;
                }
            }

            // Tile the pattern across the entire grid starting from (0,0)
            // Uses modulo to repeat the tile unit seamlessly
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    int patternI = i % patternWidth;
                    int patternJ = j % patternHeight;
                    squaresState[i, j] = pattern[patternI, patternJ];
                    cellColor[i, j] = patternColors[patternI, patternJ];
                }
            }

            // Reset generation and count alive cells
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            AliveCount = 0;

            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    if (squaresState[i, j])
                        AliveCount++;
                }
            }

            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render grid
            RenderGrid();

            // Only exit selection mode for temporary Ctrl+drag; stay in selection mode for persistent T-key
            if (isTemporarySelectionMode)
            {
                ExitSelectionMode();
            }
            else
            {
                // Clear selection but stay in selection mode (T-key persistent mode)
                selectionRect = Rectangle.Empty;
                selectionStart = Point.Empty;
                selectionEnd = Point.Empty;
                isSelecting = false;
                gridDisplay.Invalidate();
            }

            MessageBox.Show($"Pattern tiled successfully!\n{patternWidth}×{patternHeight} tile unit repeated across grid.\n{AliveCount} cells alive.",
                           "Tiling Complete",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
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

        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            // Handle paste mode click
            if (pasteMode && e.Button == MouseButtons.Left)
            {
                int cellX = e.X / cellSpacing;
                int cellY = e.Y / cellSpacing;
                if (cellX >= 0 && cellX < squarePerLine && cellY >= 0 && cellY < squarePerColumn)
                {
                    PasteAtPosition(cellX, cellY);
                }
                return;
            }

            // Handle paste mode right-click (exit)
            if (pasteMode && e.Button == MouseButtons.Right)
            {
                ExitPasteMode();
                return;
            }

            // Check if Ctrl+Left-drag should start selection (temporary mode)
            if (isCtrlKeyHeld && e.Button == MouseButtons.Left && currentMode == InteractionMode.Drawing)
            {
                // Temporarily enter selection mode for Ctrl+drag
                EnterTemporarySelectionMode();
                isSelecting = true;
                selectionStart = e.Location;
                selectionEnd = e.Location;
                selectionRect = new Rectangle(selectionStart.X, selectionStart.Y, 0, 0);
                gridDisplay.Invalidate();
                return;
            }

            if (currentMode == InteractionMode.TilingSelection)
            {
                // Start selection in persistent mode (T-key activated)
                if (e.Button == MouseButtons.Left)
                {
                    isSelecting = true;
                    selectionStart = e.Location;
                    selectionEnd = e.Location;
                    selectionRect = new Rectangle(selectionStart.X, selectionStart.Y, 0, 0);
                    gridDisplay.Invalidate();
                }
            }
            else // Drawing mode
            {
                isDragging = true;
                if (e.Button == MouseButtons.Left)
                    dragPaintMode = true;
                else if (e.Button == MouseButtons.Right)
                    dragPaintMode = false;
                else
                    return;
                PaintCell(e.X, e.Y);
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            int cellI = e.X / cellSpacing;
            int cellJ = e.Y / cellSpacing;
            if (cellI != currentMouseCell.X || cellJ != currentMouseCell.Y)
            {
                currentMouseCell = new Point(cellI, cellJ);

                // Update paste preview position
                if (pasteMode && cellI >= 0 && cellI < squarePerLine && cellJ >= 0 && cellJ < squarePerColumn)
                {
                    pastePreviewCell = new Point(cellI, cellJ);
                }

                gridDisplay.Invalidate();
            }

            if (currentMode == InteractionMode.TilingSelection && isSelecting)
            {
                // Update selection rectangle
                selectionEnd = e.Location;

                int x = Math.Min(selectionStart.X, selectionEnd.X);
                int y = Math.Min(selectionStart.Y, selectionEnd.Y);
                int width = Math.Abs(selectionEnd.X - selectionStart.X);
                int height = Math.Abs(selectionEnd.Y - selectionStart.Y);

                selectionRect = new Rectangle(x, y, width, height);
                gridDisplay.Invalidate();
            }
            else if (currentMode == InteractionMode.Drawing && isDragging)
            {
                PaintCell(e.X, e.Y);
            }
        }

        private void Grid_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentMode == InteractionMode.TilingSelection && isSelecting)
            {
                // Finalize selection - user can now press Enter to apply or keep selecting
                isSelecting = false;
            }
            else if (currentMode == InteractionMode.Drawing)
            {
                isDragging = false;
            }
        }

        private void Grid_MouseWheel(object sender, MouseEventArgs e)
        {
            // Only adjust density when Ctrl is held (plain wheel scrolls the grid panel)
            if (Control.ModifierKeys.HasFlag(Keys.Control))
            {
                // Calculate how many cells to add or remove (5% of current population, minimum 1)
                int cellChangeAmount = Math.Max(1, (int)Math.Round(AliveCount * 0.05));

                if (e.Delta > 0)
                {
                    // Scroll up: increase density (add cells)
                    AdjustPatternDensity(cellChangeAmount);
                    populationDensity = Math.Min(100, populationDensity + 5); // Update display value
                }
                else if (e.Delta < 0)
                {
                    // Scroll down: decrease density (remove cells)
                    AdjustPatternDensity(-cellChangeAmount);
                    populationDensity = Math.Max(0, populationDensity - 5); // Update display value
                }

                showDensityOverlay = true;
                densityOverlayTimer.Stop();
                densityOverlayTimer.Start();
                gridDisplay.Invalidate();
            }
            // If Ctrl not held, event bubbles up to gridPanel for scrolling (default behavior)
        }

        /// <summary>
        /// Adjusts the density of the current pattern by adding or removing cells.
        /// When adding cells, they spawn near existing alive cells (localized growth).
        /// When removing cells, they are randomly selected from alive cells.
        /// This preserves and organically extends the existing pattern structure.
        /// </summary>
        private void AdjustPatternDensity(int cellDelta)
        {
            Random random = new Random();
            List<(int, int)> changedCells = new List<(int, int)>();

            if (cellDelta > 0)
            {
                // Add cells near existing alive cells (localized growth)
                int cellsToAdd = cellDelta;

                // Build list of currently alive cells
                List<(int i, int j)> aliveCells = new List<(int, int)>();
                for (int i = 0; i < squarePerLine; i++)
                {
                    for (int j = 0; j < squarePerColumn; j++)
                    {
                        if (squaresState[i, j])
                            aliveCells.Add((i, j));
                    }
                }

                // If no alive cells, add randomly (fallback for empty grid)
                if (aliveCells.Count == 0)
                {
                    int i = random.Next(squarePerLine);
                    int j = random.Next(squarePerColumn);
                    squaresState[i, j] = true;
                    AliveCount++;
                    if (currentColorMode == ColorMode.BirthGeneration)
                        cellColor[i, j] = squareModelAlive.BackColor;
                    else
                    {
                        cellAge[i, j] = 0;
                        int colorIndex = 240;
                        cellColor[i, j] = Color.FromArgb(
                            ColorPalettes.Spectrum360[colorIndex].red,
                            ColorPalettes.Spectrum360[colorIndex].green,
                            ColorPalettes.Spectrum360[colorIndex].blue);
                    }
                    changedCells.Add((i, j));
                    aliveCells.Add((i, j));
                    cellsToAdd--;
                }

                // Add cells near existing alive cells
                int searchRadius = 3; // Search within 3 cells of an alive cell
                int maxAttemptsPerCell = 50; // Try 50 times to find a spot near each seed

                while (cellsToAdd > 0 && aliveCells.Count > 0)
                {
                    // Pick a random alive cell as seed
                    var seed = aliveCells[random.Next(aliveCells.Count)];

                    // Try to find a dead cell nearby
                    bool foundSpot = false;
                    for (int attempt = 0; attempt < maxAttemptsPerCell && !foundSpot; attempt++)
                    {
                        // Random offset within search radius
                        int di = random.Next(-searchRadius, searchRadius + 1);
                        int dj = random.Next(-searchRadius, searchRadius + 1);

                        // Calculate target position with toroidal wrapping
                        int targetI = (seed.i + di + squarePerLine) % squarePerLine;
                        int targetJ = (seed.j + dj + squarePerColumn) % squarePerColumn;

                        // Check if dead cell
                        if (!squaresState[targetI, targetJ])
                        {
                            squaresState[targetI, targetJ] = true;
                            AliveCount++;
                            if (currentColorMode == ColorMode.BirthGeneration)
                                cellColor[targetI, targetJ] = squareModelAlive.BackColor;
                            else
                            {
                                cellAge[targetI, targetJ] = 0;
                                int colorIndex = 240;
                                cellColor[targetI, targetJ] = Color.FromArgb(
                                    ColorPalettes.Spectrum360[colorIndex].red,
                                    ColorPalettes.Spectrum360[colorIndex].green,
                                    ColorPalettes.Spectrum360[colorIndex].blue);
                            }
                            changedCells.Add((targetI, targetJ));
                            aliveCells.Add((targetI, targetJ)); // Add to list so it can be a seed for next cells
                            cellsToAdd--;
                            foundSpot = true;
                        }
                    }

                    // If we couldn't find a spot near this seed after many attempts,
                    // the pattern might be fully dense in this area, so we'll try a different seed next iteration
                    if (!foundSpot)
                    {
                        // Prevent infinite loop: if we've tried all seeds, give up
                        if (aliveCells.Count >= AliveCount * 0.9) // Pattern is very dense
                            break;
                    }
                }
            }
            else if (cellDelta < 0)
            {
                // Remove cells randomly from alive cells
                int cellsToRemove = Math.Abs(cellDelta);
                cellsToRemove = Math.Min(cellsToRemove, AliveCount);
                int attempts = 0;
                int maxAttempts = cellsToRemove * 150;
                while (cellsToRemove > 0 && attempts < maxAttempts)
                {
                    int i = random.Next(squarePerLine);
                    int j = random.Next(squarePerColumn);
                    if (squaresState[i, j])
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

            if (changedCells.Count > 0)
            {
                toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
                RenderGridDirty(changedCells);
            }
        }

        private void Grid_MouseEnter(object sender, EventArgs e)
        {
            mouseOverGrid = true;
            UpdateCursorForCtrlKey();
            if (isCtrlKeyHeld)
            {
                UpdateModeIndicatorForCtrl();
            }
            gridDisplay.Invalidate();
        }

        private void Grid_MouseLeave(object sender, EventArgs e)
        {
            mouseOverGrid = false;
            currentMouseCell = new Point(-1, -1);
            if (isCtrlKeyHeld)
            {
                RestoreModeIndicator();
            }
            gridDisplay.Invalidate();
        }

        private void PaintCell(int mouseX, int mouseY)
        {
            int centerI = mouseX / cellSpacing;
            int centerJ = mouseY / cellSpacing;
            if (centerI < 0 || centerI >= squarePerLine || centerJ < 0 || centerJ >= squarePerColumn)
                return;

            List<(int, int)> cellsToUpdate = new List<(int, int)>();
            if (dragPaintMode)
            {
                if (PaintSingleCell(centerI, centerJ, true))
                    cellsToUpdate.Add((centerI, centerJ));
            }
            else
            {
                for (int di = -2; di <= 2; di++)
                {
                    for (int dj = -2; dj <= 2; dj++)
                    {
                        int i = centerI + di;
                        int j = centerJ + dj;
                        if (i >= 0 && i < squarePerLine && j >= 0 && j < squarePerColumn)
                        {
                            if (PaintSingleCell(i, j, false))
                                cellsToUpdate.Add((i, j));
                        }
                    }
                }
            }

            if (cellsToUpdate.Count > 0)
            {
                toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
                RenderGridDirty(cellsToUpdate);
            }
        }

        private bool PaintSingleCell(int i, int j, bool shouldBeAlive)
        {
            bool wasAlive = squaresState[i, j];
            if (wasAlive == shouldBeAlive) return false;

            squaresState[i, j] = shouldBeAlive;
            if (shouldBeAlive)
            {
                AliveCount++;
                if (currentColorMode == ColorMode.BirthGeneration)
                    cellColor[i, j] = squareModelAlive.BackColor;
                else
                {
                    cellAge[i, j] = 0;
                    int colorIndex = 240;
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

            stableConsecutiveCount = 0;
            PreviousAliveCount = AliveCount;
            return true;
        }

        private void Grid_Paint(object sender, PaintEventArgs e)
        {
            // Draw paste preview ghost
            if (pasteMode && clipboard != null && pastePreviewCell.X >= 0 && pastePreviewCell.Y >= 0)
            {
                using (Brush ghostBrush = new SolidBrush(Color.FromArgb(100, 0, 255, 0))) // Semi-transparent green
                using (Pen ghostPen = new Pen(Color.FromArgb(180, 0, 200, 0), 2))
                {
                    for (int i = 0; i < clipboardWidth; i++)
                    {
                        for (int j = 0; j < clipboardHeight; j++)
                        {
                            if (clipboard[i, j])
                            {
                                int targetX = (pastePreviewCell.X + i) % squarePerLine;
                                int targetY = (pastePreviewCell.Y + j) % squarePerColumn;
                                int x = targetX * cellSpacing;
                                int y = targetY * cellSpacing;
                                e.Graphics.FillRectangle(ghostBrush, x, y, squareSize, squareSize);
                                e.Graphics.DrawRectangle(ghostPen, x, y, squareSize, squareSize);
                            }
                        }
                    }
                }

                // Draw paste info
                string pasteInfo = $"{clipboardWidth}×{clipboardHeight} (H=FlipH, V=FlipV, R=Rotate90)";
                using (Font font = new Font("Segoe UI", 12, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.White))
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(220, 0, 150, 0)))
                {
                    SizeF textSize = e.Graphics.MeasureString(pasteInfo, font);
                    int infoX = pastePreviewCell.X * cellSpacing;
                    int infoY = pastePreviewCell.Y * cellSpacing - 25;
                    if (infoY < 0) infoY = pastePreviewCell.Y * cellSpacing + clipboardHeight * cellSpacing + 5;

                    e.Graphics.FillRectangle(bgBrush, infoX - 5, infoY - 2, textSize.Width + 10, textSize.Height + 4);
                    e.Graphics.DrawString(pasteInfo, font, textBrush, infoX, infoY);
                }
            }

            // Draw selection rectangle if in tiling mode
            if (currentMode == InteractionMode.TilingSelection && !selectionRect.IsEmpty)
            {
                using (Pen selectionPen = new Pen(Color.Blue, 3))
                {
                    selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(selectionPen, selectionRect);
                }

                // Draw selection info
                int cellWidth = selectionRect.Width / cellSpacing;
                int cellHeight = selectionRect.Height / cellSpacing;
                string selectionInfo = $"{cellWidth}×{cellHeight} cells";
                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.White))
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 255)))
                {
                    SizeF textSize = e.Graphics.MeasureString(selectionInfo, font);
                    int infoX = selectionRect.X + selectionRect.Width / 2 - (int)(textSize.Width / 2);
                    int infoY = selectionRect.Y - 30;
                    if (infoY < 0) infoY = selectionRect.Y + 5;

                    e.Graphics.FillRectangle(bgBrush, infoX - 5, infoY - 2, textSize.Width + 10, textSize.Height + 4);
                    e.Graphics.DrawString(selectionInfo, font, textBrush, infoX, infoY);
                }
            }

            if (showDensityOverlay && mouseOverGrid)
            {
                string densityText = $"Density: {populationDensity}%";
                using (Font font = new Font("Segoe UI", 16, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.Black))
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(220, 255, 200, 0)))
                {
                    SizeF textSize = e.Graphics.MeasureString(densityText, font);
                    Point mousePos = gridDisplay.PointToClient(Cursor.Position);
                    int overlayX = mousePos.X + 20;
                    int overlayY = mousePos.Y - 30;
                    e.Graphics.FillRectangle(bgBrush, overlayX, overlayY, textSize.Width + 10, textSize.Height + 5);
                    e.Graphics.DrawString(densityText, font, textBrush, overlayX + 5, overlayY + 2);
                }
            }

            if (!mouseOverGrid || currentMouseCell.X < 0 || currentMouseCell.Y < 0) return;
            int cellI = currentMouseCell.X;
            int cellJ = currentMouseCell.Y;
            if (cellI < 0 || cellI >= squarePerLine || cellJ < 0 || cellJ >= squarePerColumn) return;

            using (Pen cursorPen = new Pen(Color.Black, 2))
            {
                if (isDragging && !dragPaintMode)
                {
                    int eraserRadius = 2;
                    int startI = Math.Max(0, cellI - eraserRadius);
                    int startJ = Math.Max(0, cellJ - eraserRadius);
                    int endI = Math.Min(squarePerLine - 1, cellI + eraserRadius);
                    int endJ = Math.Min(squarePerColumn - 1, cellJ + eraserRadius);
                    int x = startI * cellSpacing - 1;
                    int y = startJ * cellSpacing - 1;
                    int width = (endI - startI + 1) * cellSpacing + 2;
                    int height = (endJ - startJ + 1) * cellSpacing + 2;
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

        /// <summary>
        /// Copies the current selection to the clipboard.
        /// </summary>
        private void CopySelection()
        {
            if (selectionRect.IsEmpty || selectionRect.Width == 0 || selectionRect.Height == 0)
            {
                MessageBox.Show("No region selected. Use Ctrl+Drag or T-mode to select a region first.",
                               "Copy Selection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
                return;
            }

            // Convert pixel coordinates to cell coordinates
            int startCellX = selectionRect.X / cellSpacing;
            int startCellY = selectionRect.Y / cellSpacing;
            int endCellX = (selectionRect.X + selectionRect.Width) / cellSpacing;
            int endCellY = (selectionRect.Y + selectionRect.Height) / cellSpacing;

            // Ensure bounds
            startCellX = Math.Max(0, startCellX);
            startCellY = Math.Max(0, startCellY);
            endCellX = Math.Min(squarePerLine, endCellX);
            endCellY = Math.Min(squarePerColumn, endCellY);

            clipboardWidth = endCellX - startCellX;
            clipboardHeight = endCellY - startCellY;

            if (clipboardWidth <= 0 || clipboardHeight <= 0)
            {
                MessageBox.Show("Selected region is too small.",
                               "Copy Selection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            // Allocate clipboard arrays
            clipboard = new bool[clipboardWidth, clipboardHeight];
            clipboardColors = new Color[clipboardWidth, clipboardHeight];

            // Copy cells to clipboard
            for (int i = 0; i < clipboardWidth; i++)
            {
                for (int j = 0; j < clipboardHeight; j++)
                {
                    clipboard[i, j] = squaresState[startCellX + i, startCellY + j];
                    clipboardColors[i, j] = cellColor[startCellX + i, startCellY + j];
                }
            }

            toolStripModeIndicator.Text = $"Copied {clipboardWidth}×{clipboardHeight}";
            toolStripModeIndicator.BackColor = Color.LightYellow;

            // Update Edit menu state
            UpdateEditMenuState();

            // Reset after 2 seconds
            System.Threading.Tasks.Task.Delay(2000).ContinueWith(_ =>
            {
                this.Invoke((Action)(() =>
                {
                    if (pasteMode)
                    {
                        toolStripModeIndicator.Text = "Paste Mode (Click to paste, Esc to exit)";
                        toolStripModeIndicator.BackColor = Color.LightCoral;
                    }
                    else if (currentMode == InteractionMode.Drawing)
                    {
                        RestoreModeIndicator();
                    }
                }));
            });
        }

        /// <summary>
        /// Enters paste mode with visual preview.
        /// </summary>
        private void EnterPasteMode()
        {
            pasteMode = true;
            pastePreviewCell = new Point(-1, -1);

            toolStripModeIndicator.Text = "Paste Mode (Click to paste, Esc to exit)";
            toolStripModeIndicator.BackColor = Color.LightCoral;

            gridDisplay.Cursor = Cursors.Cross;

            // Update Edit menu state
            UpdateEditMenuState();

            gridDisplay.Invalidate();
        }

        /// <summary>
        /// Exits paste mode and returns to previous mode.
        /// </summary>
        private void ExitPasteMode()
        {
            pasteMode = false;
            pastePreviewCell = new Point(-1, -1);

            if (currentMode == InteractionMode.TilingSelection)
            {
                toolStripModeIndicator.Text = "Mode: Select Region (T)";
                toolStripModeIndicator.BackColor = Color.LightSkyBlue;
                gridDisplay.Cursor = Cursors.Cross;
            }
            else
            {
                RestoreModeIndicator();
                UpdateCursorForCtrlKey();
            }

            // Update Edit menu state
            UpdateEditMenuState();

            gridDisplay.Invalidate();
        }

        /// <summary>
        /// Pastes clipboard content at the specified cell position.
        /// </summary>
        private void PasteAtPosition(int cellX, int cellY)
        {
            if (clipboard == null || clipboardWidth == 0 || clipboardHeight == 0)
                return;

            List<(int, int)> changedCells = new List<(int, int)>();

            for (int i = 0; i < clipboardWidth; i++)
            {
                for (int j = 0; j < clipboardHeight; j++)
                {
                    int targetX = cellX + i;
                    int targetY = cellY + j;

                    // Wrap around grid (toroidal)
                    targetX = (targetX + squarePerLine) % squarePerLine;
                    targetY = (targetY + squarePerColumn) % squarePerColumn;

                    bool oldState = squaresState[targetX, targetY];
                    bool newState = clipboard[i, j];

                    if (oldState != newState)
                    {
                        squaresState[targetX, targetY] = newState;
                        cellColor[targetX, targetY] = clipboardColors[i, j];

                        if (newState)
                            AliveCount++;
                        else
                            AliveCount = Math.Max(0, AliveCount - 1);

                        changedCells.Add((targetX, targetY));
                    }
                    else if (newState) // Cell already alive, update color
                    {
                        cellColor[targetX, targetY] = clipboardColors[i, j];
                        changedCells.Add((targetX, targetY));
                    }
                }
            }

            if (changedCells.Count > 0)
            {
                toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
                RenderGridDirty(changedCells);
            }
        }

        /// <summary>
        /// Flips clipboard horizontally (mirrors left-right).
        /// </summary>
        private void FlipClipboardHorizontal()
        {
            if (clipboard == null) return;

            bool[,] flipped = new bool[clipboardWidth, clipboardHeight];
            Color[,] flippedColors = new Color[clipboardWidth, clipboardHeight];

            for (int i = 0; i < clipboardWidth; i++)
            {
                for (int j = 0; j < clipboardHeight; j++)
                {
                    flipped[i, j] = clipboard[clipboardWidth - 1 - i, j];
                    flippedColors[i, j] = clipboardColors[clipboardWidth - 1 - i, j];
                }
            }

            clipboard = flipped;
            clipboardColors = flippedColors;
        }

        /// <summary>
        /// Flips clipboard vertically (mirrors top-bottom).
        /// </summary>
        private void FlipClipboardVertical()
        {
            if (clipboard == null) return;

            bool[,] flipped = new bool[clipboardWidth, clipboardHeight];
            Color[,] flippedColors = new Color[clipboardWidth, clipboardHeight];

            for (int i = 0; i < clipboardWidth; i++)
            {
                for (int j = 0; j < clipboardHeight; j++)
                {
                    flipped[i, j] = clipboard[i, clipboardHeight - 1 - j];
                    flippedColors[i, j] = clipboardColors[i, clipboardHeight - 1 - j];
                }
            }

            clipboard = flipped;
            clipboardColors = flippedColors;
        }

        /// <summary>
        /// Rotates clipboard 90 degrees clockwise.
        /// </summary>
        private void RotateClipboard90CW()
        {
            if (clipboard == null) return;

            // Swap dimensions
            int newWidth = clipboardHeight;
            int newHeight = clipboardWidth;

            bool[,] rotated = new bool[newWidth, newHeight];
            Color[,] rotatedColors = new Color[newWidth, newHeight];

            for (int i = 0; i < clipboardWidth; i++)
            {
                for (int j = 0; j < clipboardHeight; j++)
                {
                    rotated[clipboardHeight - 1 - j, i] = clipboard[i, j];
                    rotatedColors[clipboardHeight - 1 - j, i] = clipboardColors[i, j];
                }
            }

            clipboard = rotated;
            clipboardColors = rotatedColors;
            clipboardWidth = newWidth;
            clipboardHeight = newHeight;
        }

        /// <summary>
        /// Rotates clipboard 90 degrees counter-clockwise.
        /// </summary>
        private void RotateClipboard90CCW()
        {
            if (clipboard == null) return;

            // Swap dimensions
            int newWidth = clipboardHeight;
            int newHeight = clipboardWidth;

            bool[,] rotated = new bool[newWidth, newHeight];
            Color[,] rotatedColors = new Color[newWidth, newHeight];

            for (int i = 0; i < clipboardWidth; i++)
            {
                for (int j = 0; j < clipboardHeight; j++)
                {
                    rotated[j, clipboardWidth - 1 - i] = clipboard[i, j];
                    rotatedColors[j, clipboardWidth - 1 - i] = clipboardColors[i, j];
                }
            }

            clipboard = rotated;
            clipboardColors = rotatedColors;
            clipboardWidth = newWidth;
            clipboardHeight = newHeight;
        }

        /// <summary>
        /// Rotates clipboard 180 degrees.
        /// </summary>
        private void RotateClipboard180()
        {
            FlipClipboardHorizontal();
            FlipClipboardVertical();
        }

        private void colorModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorMode newMode = colorModeComboBox.SelectedIndex == 0 ? ColorMode.BirthGeneration : ColorMode.CellAging;
            if (newMode != currentColorMode)
            {
                currentColorMode = newMode;
                if (currentColorMode == ColorMode.CellAging)
                {
                    toolStripIterationsTextbox.BackColor = Color.White;
                    toolStripIterationsTextbox.ForeColor = Color.Black;
                }
                else
                {
                    toolStripIterationsTextbox.BackColor = squareModelAlive.BackColor;
                    Color InverseColor = Color.FromArgb(255 - squareModelAlive.BackColor.R,
                        255 - squareModelAlive.BackColor.G, 255 - squareModelAlive.BackColor.B);
                    toolStripIterationsTextbox.ForeColor = InverseColor;
                }
                RecolorCellsForMode();
                RenderGrid();
            }
        }

        private void RecolorCellsForMode()
        {
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    if (squaresState[i, j])
                    {
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
                        }
                        else
                        {
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

        /// <summary>
        /// Loads a chessboard tiling pattern with 5x5 blocks.
        /// </summary>
        private void chessboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadChessboardPattern(5);
        }

        /// <summary>
        /// Generates a chessboard tiling pattern across the entire grid.
        /// </summary>
        /// <param name="blockSize">Size of each square block in the chessboard pattern.</param>
        private void LoadChessboardPattern(int blockSize)
        {
            // Clear the grid first
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Create chessboard pattern
            AliveCount = 0;
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    // Determine which block this cell belongs to
                    int blockI = i / blockSize;
                    int blockJ = j / blockSize;

                    // Chessboard pattern: alternate blocks based on sum of block indices
                    bool shouldBeAlive = (blockI + blockJ) % 2 == 0;

                    if (shouldBeAlive)
                    {
                        squaresState[i, j] = true;
                        AliveCount++;

                        // Set color based on current mode
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
                        }
                        else // CellAging mode
                        {
                            cellAge[i, j] = 0;
                            int colorIndex = 240; // Blue (0,0,255)
                            cellColor[i, j] = Color.FromArgb(
                                ColorPalettes.Spectrum360[colorIndex].red,
                                ColorPalettes.Spectrum360[colorIndex].green,
                                ColorPalettes.Spectrum360[colorIndex].blue);
                        }
                    }
                }
            }

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the pattern
            RenderGrid();

            MessageBox.Show($"Chessboard pattern ({blockSize}x{blockSize} blocks) loaded!\n{AliveCount} cells alive.",
                           "Pattern Loaded",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }

        /// <summary>
        /// Loads a brick/running bond tiling pattern.
        /// </summary>
        private void brickPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadBrickPattern(5, 3);
        }

        /// <summary>
        /// Loads diagonal stripes pattern.
        /// </summary>
        private void diagonalStripesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDiagonalStripesPattern(5);
        }

        /// <summary>
        /// Loads double diagonal (cross-hatch) pattern.
        /// </summary>
        private void doubleDiagonalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDoubleDiagonalPattern(5);
        }

        /// <summary>
        /// Loads concentric rectangles pattern.
        /// </summary>
        private void concentricRectanglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadConcentricRectanglesPattern(3);
        }

        /// <summary>
        /// Generates a brick/running bond tiling pattern.
        /// Alternating rows are offset, creating a brick wall appearance.
        /// </summary>
        /// <param name="brickWidth">Width of each brick.</param>
        /// <param name="brickHeight">Height of each brick.</param>

        /// <summary>
        /// Generates diagonal stripe pattern across the grid.
        /// Creates diagonal bands that often spawn gliders and spaceships.
        /// </summary>
        /// <param name="stripeWidth">Width of each stripe in cells.</param>
        private void LoadDiagonalStripesPattern(int stripeWidth)
        {
            // Clear the grid first
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Create diagonal stripes pattern
            AliveCount = 0;
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    // Diagonal pattern: use sum of coordinates modulo stripe width
                    int diagonalIndex = (i + j) / stripeWidth;
                    bool shouldBeAlive = diagonalIndex % 2 == 0;

                    if (shouldBeAlive)
                    {
                        squaresState[i, j] = true;
                        AliveCount++;

                        // Set color based on current mode
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
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
                    }
                }
            }

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the pattern
            RenderGrid();

            MessageBox.Show($"Diagonal stripes pattern (width {stripeWidth}) loaded!\n{AliveCount} cells alive.",
                   "Pattern Loaded",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
        }

        /// <summary>
        /// Generates double diagonal (cross-hatch) pattern across the grid.
        /// Creates diamond lattice patterns with diagonals running in both directions.
        /// Maintains beautiful geometric structure while evolving gracefully.
        /// </summary>
        /// <param name="stripeWidth">Width of each diagonal stripe in cells.</param>
        private void LoadDoubleDiagonalPattern(int stripeWidth)
        {
            // Clear the grid first
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Create double diagonal (cross-hatch) lattice pattern
            AliveCount = 0;
            int diagonalSpacing = 12; // Distance between parallel diagonals
            int diagonalWidth = 3; // Width of each diagonal line (3 cells wide for stability)

            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    // First diagonal family: upper-left to lower-right (using i + j)
                    int diagonal1Pos = (i + j) % diagonalSpacing;
                    bool onDiagonal1 = diagonal1Pos < diagonalWidth;

                    // Second diagonal family: lower-left to upper-right (using i - j)
                    int diagonal2Pos = (i - j + squarePerColumn * 2) % diagonalSpacing;
                    bool onDiagonal2 = diagonal2Pos < diagonalWidth;

                    // Cell is on a diagonal if it's on either family
                    bool onAnyDiagonal = onDiagonal1 || onDiagonal2;

                    if (onAnyDiagonal)
                    {
                        bool shouldBeAlive = true;

                        // Create very sparse, regular gaps for controlled evolution
                        // Only create gaps at specific intervals to maintain structure
                        int positionAlongDiagonal = (i + j) / 2;

                        // Small gaps every 15 cells along the diagonal (very sparse)
                        bool smallGap = (positionAlongDiagonal % 15 == 7);

                        // At intersection points, always keep cells alive for lattice continuity
                        bool atIntersection = onDiagonal1 && onDiagonal2;

                        if (atIntersection)
                        {
                            // Intersections are always alive - they anchor the lattice
                            shouldBeAlive = true;
                        }
                        else if (smallGap && !atIntersection)
                        {
                            // Create a small gap, but only away from intersections
                            shouldBeAlive = false;
                        }

                        // Add a few seed patterns at specific locations for gliders
                        // But only in the middle cell of each diagonal stripe
                        if (onDiagonal1 && diagonal1Pos == 1 && (i + j) % 24 == 12)
                        {
                            shouldBeAlive = false; // Strategic gap for oscillator formation
                        }

                        if (shouldBeAlive)
                        {
                            squaresState[i, j] = true;
                            AliveCount++;

                            // Set color based on current mode
                            if (currentColorMode == ColorMode.BirthGeneration)
                            {
                                cellColor[i, j] = squareModelAlive.BackColor;
                                cellAge[i, j] = 0;
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
                        }
                    }
                }
            }

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the pattern
            RenderGrid();

            MessageBox.Show($"Double diagonal lattice loaded!\n{AliveCount} cells alive.\n\nWatch the diamond lattice evolve while maintaining its geometric beauty!",
                   "Geometric Pattern Loaded",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
        }

        /// <summary>
        /// Generates concentric rectangles expanding from the center.
        /// Creates pulsing wave-like patterns in Game of Life.
        /// </summary>
        /// <param name="spacing">Spacing between consecutive rectangles.</param>
        private void LoadConcentricRectanglesPattern(int spacing)
        {
            // Clear the grid first
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Find center of grid
            int centerX = squarePerLine / 2;
            int centerY = squarePerColumn / 2;

            // Create concentric rectangles
            AliveCount = 0;
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    // Calculate distance from center using Chebyshev distance (max of dx, dy)
                    int dx = Math.Abs(i - centerX);
                    int dy = Math.Abs(j - centerY);
                    int distance = Math.Max(dx, dy);

                    // Create rings at specific distances
                    bool shouldBeAlive = (distance / spacing) % 2 == 0;

                    if (shouldBeAlive)
                    {
                        squaresState[i, j] = true;
                        AliveCount++;

                        // Set color based on current mode
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
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
                    }
                }
            }

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the pattern
            RenderGrid();

            MessageBox.Show($"Concentric rectangles pattern (spacing {spacing}) loaded!\n{AliveCount} cells alive.",
                   "Pattern Loaded",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
        }

        /// <summary>
        /// Generates a "Ladder Brick" pattern - brick-like structure with strategic gaps.
        /// Creates oscillators, gliders, and evolving patterns while maintaining structure.
        /// Much more dynamic and long-lasting than traditional brick patterns.
        /// </summary>
        /// <param name="brickWidth">Width of each brick section (default 6).</param>
        /// <param name="brickHeight">Height of each brick (default 2).</param>
        private void LoadBrickPattern(int brickWidth = 6, int brickHeight = 2)
        {
            // Clear the grid first
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Pattern spacing parameters for dynamic behavior
            int ladderSpacing = 8; // Space between ladder rungs
            int railWidth = 1; // Width of vertical supports
            int rungGap = 2; // Gap in each rung to create oscillators

            // Create ladder brick pattern
            AliveCount = 0;
            for (int j = 0; j < squarePerColumn; j += ladderSpacing)
            {
                for (int jOffset = 0; jOffset < brickHeight && (j + jOffset) < squarePerColumn; jOffset++)
                {
                    int currentRow = j + jOffset;

                    // Draw horizontal "rungs" with strategic gaps
                    for (int i = 0; i < squarePerLine; i++)
                    {
                        // Create brick-like sections with gaps for oscillators
                        int sectionPos = i % (brickWidth + rungGap);

                        // Draw brick section (not the gap)
                        if (sectionPos < brickWidth)
                        {
                            // Add some randomness to make it more interesting
                            // Skip some cells to create seed patterns
                            bool shouldPlace = true;

                            // Create vertical "supports" every 20 cells
                            if (i % 20 == 0 && jOffset == 0)
                            {
                                // This creates vertical elements
                                shouldPlace = true;
                            }
                            // Create strategic gaps for blinker/toad formation
                            else if (sectionPos == brickWidth / 2 && (i / (brickWidth + rungGap)) % 3 == 0)
                            {
                                shouldPlace = false; // Gap for oscillator
                            }

                            if (shouldPlace)
                            {
                                squaresState[i, currentRow] = true;
                                AliveCount++;

                                // Set color
                                if (currentColorMode == ColorMode.BirthGeneration)
                                {
                                    cellColor[i, currentRow] = squareModelAlive.BackColor;
                                    cellAge[i, currentRow] = 0;
                                }
                                else
                                {
                                    cellAge[i, currentRow] = 0;
                                    int colorIndex = 240;
                                    cellColor[i, currentRow] = Color.FromArgb(
                                        ColorPalettes.Spectrum360[colorIndex].red,
                                        ColorPalettes.Spectrum360[colorIndex].green,
                                        ColorPalettes.Spectrum360[colorIndex].blue);
                                }
                            }
                        }
                    }
                }

                // Add vertical "rails" between rungs to connect structure
                if (j + brickHeight + 2 < squarePerColumn)
                {
                    for (int i = 0; i < squarePerLine; i += (brickWidth + rungGap))
                    {
                        // Place small vertical connector
                        for (int k = 0; k < 2 && (j + brickHeight + k) < squarePerColumn; k++)
                        {
                            if (i < squarePerLine)
                            {
                                squaresState[i, j + brickHeight + k] = true;
                                AliveCount++;

                                if (currentColorMode == ColorMode.BirthGeneration)
                                {
                                    cellColor[i, j + brickHeight + k] = squareModelAlive.BackColor;
                                    cellAge[i, j + brickHeight + k] = 0;
                                }
                                else
                                {
                                    cellAge[i, j + brickHeight + k] = 0;
                                    int colorIndex = 240;
                                    cellColor[i, j + brickHeight + k] = Color.FromArgb(
                                        ColorPalettes.Spectrum360[colorIndex].red,
                                        ColorPalettes.Spectrum360[colorIndex].green,
                                        ColorPalettes.Spectrum360[colorIndex].blue);
                                }
                            }
                        }
                    }
                }
            }

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the pattern
            RenderGrid();

            MessageBox.Show($"Ladder Brick pattern loaded!\n{AliveCount} cells alive.\n\nThis pattern creates oscillators, gliders, and evolving shapes!",
                   "Dynamic Pattern Loaded",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
        }

        /// <summary>
        /// Loads thin diagonal stripes pattern (1-2 cell width).
        /// Creates many gliders and long-lived evolution with symmetric appearance.
        /// </summary>
        private void thinDiagonalStripesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadThinDiagonalStripesPattern();
        }

        /// <summary>
        /// Loads a grid of Pulsar patterns.
        /// Creates beautiful synchronized period-3 oscillation across the entire grid.
        /// </summary>
        private void pulsarGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPulsarGridPattern();
        }

        /// <summary>
        /// Generates thin diagonal stripes (1-2 cells wide) that spawn gliders and create long-lived evolution.
        /// Much more dynamic than thick stripes (width=5) while maintaining visual symmetry.
        /// </summary>
        private void LoadThinDiagonalStripesPattern()
        {
            // Clear the grid first
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Thin diagonal stripes: width 1-2 cells, spacing 10 cells
            int stripeWidth = 2;  // Thin stripes (1-2 cells)
            int stripeSpacing = 10;  // Distance between stripes

            AliveCount = 0;
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    // Diagonal pattern: cells where (i+j) falls within stripe regions
                    int diagonalPosition = (i + j) % stripeSpacing;
                    bool shouldBeAlive = diagonalPosition < stripeWidth;

                    if (shouldBeAlive)
                    {
                        squaresState[i, j] = true;
                        AliveCount++;

                        // Set color based on current mode
                        if (currentColorMode == ColorMode.BirthGeneration)
                        {
                            cellColor[i, j] = squareModelAlive.BackColor;
                            cellAge[i, j] = 0;
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
                    }
                }
            }

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the pattern
            RenderGrid();

            MessageBox.Show($"Thin diagonal stripes loaded! ({stripeWidth} cells wide, {stripeSpacing} spacing)\n{AliveCount} cells alive.\n\nWatch for gliders spawning along the diagonal wavefront!",
                           "Long-Lived Pattern Loaded",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }

        /// <summary>
        /// Generates a grid of Pulsar oscillators with synchronized period-3 pulsing.
        /// Creates a beautiful, symmetric field of oscillating stars.
        /// </summary>
        private void LoadPulsarGridPattern()
        {
            // Clear the grid first
            for (int i = 0; i < squarePerLine; i++)
            {
                for (int j = 0; j < squarePerColumn; j++)
                {
                    squaresState[i, j] = false;
                    cellColor[i, j] = squareModel.BackColor;
                }
            }

            // Pulsar is 13x13, we'll tile it with spacing
            // Get Pulsar pattern from library
            var patterns = GameOfLife.PatternLibrary.GetAllPatterns();
            if (!patterns.ContainsKey("Pulsar"))
            {
                MessageBox.Show("Pulsar pattern not found in library!", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string pulsarRle = patterns["Pulsar"];
            GameOfLife.Pattern pulsarPattern = GameOfLife.RleParser.Parse(pulsarRle);

            // Pulsar dimensions (13x13) + spacing
            int pulsarSize = 13;
            int spacing = 8;  // Space between pulsars
            int tileUnit = pulsarSize + spacing;  // 21 cells per tile

            // Calculate how many pulsars fit in the grid
            int tilesX = squarePerLine / tileUnit;
            int tilesY = squarePerColumn / tileUnit;

            // Center the grid of pulsars
            int offsetX = (squarePerLine - tilesX * tileUnit) / 2;
            int offsetY = (squarePerColumn - tilesY * tileUnit) / 2;

            AliveCount = 0;

            // Tile the pulsars across the grid
            for (int tileI = 0; tileI < tilesX; tileI++)
            {
                for (int tileJ = 0; tileJ < tilesY; tileJ++)
                {
                    int startX = offsetX + tileI * tileUnit;
                    int startY = offsetY + tileJ * tileUnit;

                    // Place the pulsar pattern
                    for (int px = 0; px < pulsarPattern.Width && startX + px < squarePerLine; px++)
                    {
                        for (int py = 0; py < pulsarPattern.Height && startY + py < squarePerColumn; py++)
                        {
                            if (pulsarPattern.Cells[px, py])
                            {
                                int gridX = startX + px;
                                int gridY = startY + py;

                                if (gridX >= 0 && gridX < squarePerLine && gridY >= 0 && gridY < squarePerColumn)
                                {
                                    squaresState[gridX, gridY] = true;
                                    AliveCount++;

                                    // Set color based on current mode
                                    if (currentColorMode == ColorMode.BirthGeneration)
                                    {
                                        cellColor[gridX, gridY] = squareModelAlive.BackColor;
                                        cellAge[gridX, gridY] = 0;
                                    }
                                    else // CellAging mode
                                    {
                                        cellAge[gridX, gridY] = 0;
                                        int colorIndex = 240; // Blue
                                        cellColor[gridX, gridY] = Color.FromArgb(
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

            // Reset game state
            TickNumber = 0;
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripAliveCountBox.Text = AliveCount.ToString() + " alive.";
            PreviousAliveCount = -1;
            stableConsecutiveCount = 0;

            // Render the pattern
            RenderGrid();

            MessageBox.Show($"Pulsar Grid loaded! ({tilesX}×{tilesY} pulsars)\n{AliveCount} cells alive.\n\nWatch the synchronized period-3 oscillation create a beautiful pulsing field!",
                           "Symmetric Oscillator Field Loaded",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }

        // Edit menu event handlers

        /// <summary>
        /// Updates the enabled state of Edit menu items based on clipboard and paste mode state.
        /// </summary>
        private void UpdateEditMenuState()
        {
            if (pasteToolStripMenuItem != null)
            {
                pasteToolStripMenuItem.Enabled = (clipboard != null);
            }

            if (transformToolStripMenuItem != null)
            {
                transformToolStripMenuItem.Enabled = pasteMode && (clipboard != null);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelection();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clipboard != null)
            {
                EnterPasteMode();
            }
        }

        private void flipHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pasteMode && clipboard != null)
            {
                FlipClipboardHorizontal();
                gridDisplay.Invalidate();
            }
        }

        private void flipVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pasteMode && clipboard != null)
            {
                FlipClipboardVertical();
                gridDisplay.Invalidate();
            }
        }

        private void rotate90CWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pasteMode && clipboard != null)
            {
                RotateClipboard90CW();
                gridDisplay.Invalidate();
            }
        }

        private void rotate90CCWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pasteMode && clipboard != null)
            {
                RotateClipboard90CCW();
                gridDisplay.Invalidate();
            }
        }

        private void rotate180ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pasteMode && clipboard != null)
            {
                RotateClipboard180();
                gridDisplay.Invalidate();
            }
        }
    }
}
