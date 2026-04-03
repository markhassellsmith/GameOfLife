namespace TP14_JeudelaVie
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuEtatVitesse = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exportPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            transformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            flipHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            flipVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rotate90CWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rotate90CCWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            rotate180ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            simulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            vitesseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            middleSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rapideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            patternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            randomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tileSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            shapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            gliderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lwssToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            blinkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            beaconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pulsarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            rPentominoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            acornToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diehardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            gosperGliderGunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tilingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            chessboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            brickPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diagonalStripesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            doubleDiagonalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            concentricRectanglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            colorModeComboBox = new System.Windows.Forms.ToolStripComboBox();
            toolStripIterationsTextbox = new System.Windows.Forms.ToolStripTextBox();
            toolStripAliveCountBox = new System.Windows.Forms.ToolStripTextBox();
            toolStripModeIndicator = new System.Windows.Forms.ToolStripTextBox();
            toolStripSpacebarComment = new System.Windows.Forms.ToolStripTextBox();
            toolStripTextBoxStatus = new System.Windows.Forms.ToolStripTextBox();
            myTimer = new System.Windows.Forms.Timer(components);
            squareModel = new System.Windows.Forms.PictureBox();
            squareModelAlive = new System.Windows.Forms.PictureBox();
            menuEtatVitesse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)squareModel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)squareModelAlive).BeginInit();
            SuspendLayout();
            // 
            // menuEtatVitesse
            // 
            menuEtatVitesse.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuEtatVitesse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, simulationToolStripMenuItem, vitesseToolStripMenuItem, patternToolStripMenuItem, colorModeComboBox, toolStripIterationsTextbox, toolStripAliveCountBox, toolStripModeIndicator, toolStripSpacebarComment, toolStripTextBoxStatus });
            menuEtatVitesse.Location = new System.Drawing.Point(0, 0);
            menuEtatVitesse.Name = "menuEtatVitesse";
            menuEtatVitesse.Padding = new System.Windows.Forms.Padding(10, 3, 0, 3);
            menuEtatVitesse.Size = new System.Drawing.Size(2313, 64);
            menuEtatVitesse.TabIndex = 0;
            menuEtatVitesse.Text = "menuStrip1";
            menuEtatVitesse.ItemClicked += menuEtatVitesse_ItemClicked;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { resetToolStripMenuItem, clearGridToolStripMenuItem, exportPatternToolStripMenuItem, importPatternToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(71, 58);
            fileToolStripMenuItem.Text = "&File";
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new System.Drawing.Size(318, 44);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // clearGridToolStripMenuItem
            // 
            clearGridToolStripMenuItem.Name = "clearGridToolStripMenuItem";
            clearGridToolStripMenuItem.Size = new System.Drawing.Size(318, 44);
            clearGridToolStripMenuItem.Text = "Clear the Grid";
            clearGridToolStripMenuItem.Click += clearGridToolStripMenuItem_Click;
            // 
            // exportPatternToolStripMenuItem
            // 
            exportPatternToolStripMenuItem.Name = "exportPatternToolStripMenuItem";
            exportPatternToolStripMenuItem.Size = new System.Drawing.Size(318, 44);
            exportPatternToolStripMenuItem.Text = "Export Pattern ...";
            exportPatternToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            exportPatternToolStripMenuItem.Click += exportPatternToolStripMenuItem_Click;
            // 
            // importPatternToolStripMenuItem
            // 
            importPatternToolStripMenuItem.Name = "importPatternToolStripMenuItem";
            importPatternToolStripMenuItem.Size = new System.Drawing.Size(318, 44);
            importPatternToolStripMenuItem.Text = "Import Pattern..";
            importPatternToolStripMenuItem.Click += importPatternToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            exitToolStripMenuItem.Size = new System.Drawing.Size(318, 44);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { copyToolStripMenuItem, pasteToolStripMenuItem, toolStripSeparator3, transformToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(74, 58);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            copyToolStripMenuItem.Size = new System.Drawing.Size(290, 44);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            pasteToolStripMenuItem.Size = new System.Drawing.Size(290, 44);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(287, 6);
            // 
            // transformToolStripMenuItem
            // 
            transformToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { flipHorizontalToolStripMenuItem, flipVerticalToolStripMenuItem, rotate90CWToolStripMenuItem, rotate90CCWToolStripMenuItem, toolStripSeparator4, rotate180ToolStripMenuItem });
            transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            transformToolStripMenuItem.Size = new System.Drawing.Size(290, 44);
            transformToolStripMenuItem.Text = "Transform";
            // 
            // flipHorizontalToolStripMenuItem
            // 
            flipHorizontalToolStripMenuItem.Name = "flipHorizontalToolStripMenuItem";
            flipHorizontalToolStripMenuItem.Size = new System.Drawing.Size(350, 44);
            flipHorizontalToolStripMenuItem.Text = "Flip Horizontal (H)";
            flipHorizontalToolStripMenuItem.Click += flipHorizontalToolStripMenuItem_Click;
            // 
            // flipVerticalToolStripMenuItem
            // 
            flipVerticalToolStripMenuItem.Name = "flipVerticalToolStripMenuItem";
            flipVerticalToolStripMenuItem.Size = new System.Drawing.Size(350, 44);
            flipVerticalToolStripMenuItem.Text = "Flip Vertical (V)";
            flipVerticalToolStripMenuItem.Click += flipVerticalToolStripMenuItem_Click;
            // 
            // rotate90CWToolStripMenuItem
            // 
            rotate90CWToolStripMenuItem.Name = "rotate90CWToolStripMenuItem";
            rotate90CWToolStripMenuItem.Size = new System.Drawing.Size(350, 44);
            rotate90CWToolStripMenuItem.Text = "Rotate 90° CW (R)";
            rotate90CWToolStripMenuItem.Click += rotate90CWToolStripMenuItem_Click;
            // 
            // rotate90CCWToolStripMenuItem
            // 
            rotate90CCWToolStripMenuItem.Name = "rotate90CCWToolStripMenuItem";
            rotate90CCWToolStripMenuItem.Size = new System.Drawing.Size(350, 44);
            rotate90CCWToolStripMenuItem.Text = "Rotate 90° CCW (Shift+R)";
            rotate90CCWToolStripMenuItem.Click += rotate90CCWToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(347, 6);
            // 
            // rotate180ToolStripMenuItem
            // 
            rotate180ToolStripMenuItem.Name = "rotate180ToolStripMenuItem";
            rotate180ToolStripMenuItem.Size = new System.Drawing.Size(350, 44);
            rotate180ToolStripMenuItem.Text = "Rotate 180°";
            rotate180ToolStripMenuItem.Click += rotate180ToolStripMenuItem_Click;
            // 
            // simulationToolStripMenuItem
            // 
            simulationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem });
            simulationToolStripMenuItem.Name = "simulationToolStripMenuItem";
            simulationToolStripMenuItem.Size = new System.Drawing.Size(148, 58);
            simulationToolStripMenuItem.Text = "&Simulation";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            startToolStripMenuItem.Size = new System.Drawing.Size(275, 44);
            startToolStripMenuItem.Text = "Run";
            startToolStripMenuItem.Click += runToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            stopToolStripMenuItem.Size = new System.Drawing.Size(275, 44);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // vitesseToolStripMenuItem
            // 
            vitesseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { lentToolStripMenuItem, middleSpeedToolStripMenuItem, rapideToolStripMenuItem });
            vitesseToolStripMenuItem.Name = "vitesseToolStripMenuItem";
            vitesseToolStripMenuItem.Size = new System.Drawing.Size(101, 58);
            vitesseToolStripMenuItem.Text = "Sp&eed";
            // 
            // lentToolStripMenuItem
            // 
            lentToolStripMenuItem.Name = "lentToolStripMenuItem";
            lentToolStripMenuItem.Size = new System.Drawing.Size(237, 44);
            lentToolStripMenuItem.Text = "Slow";
            lentToolStripMenuItem.Click += slowToolStripMenuItem_Click;
            // 
            // middleSpeedToolStripMenuItem
            // 
            middleSpeedToolStripMenuItem.Name = "middleSpeedToolStripMenuItem";
            middleSpeedToolStripMenuItem.Size = new System.Drawing.Size(237, 44);
            middleSpeedToolStripMenuItem.Text = "Medium";
            middleSpeedToolStripMenuItem.Click += normalSpeedToolStripMenuItem_Click;
            // 
            // rapideToolStripMenuItem
            // 
            rapideToolStripMenuItem.Name = "rapideToolStripMenuItem";
            rapideToolStripMenuItem.Size = new System.Drawing.Size(237, 44);
            rapideToolStripMenuItem.Text = "Quick";
            rapideToolStripMenuItem.Click += quickToolStripMenuItem_Click;
            // 
            // patternToolStripMenuItem
            // 
            patternToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { randomToolStripMenuItem, tileSelectionToolStripMenuItem, shapesToolStripMenuItem, tilingsToolStripMenuItem });
            patternToolStripMenuItem.Name = "patternToolStripMenuItem";
            patternToolStripMenuItem.Size = new System.Drawing.Size(109, 58);
            patternToolStripMenuItem.Text = "&Pattern";
            // 
            // randomToolStripMenuItem
            // 
            randomToolStripMenuItem.Checked = true;
            randomToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            randomToolStripMenuItem.Size = new System.Drawing.Size(339, 44);
            randomToolStripMenuItem.Text = "Random";
            randomToolStripMenuItem.Click += randomToolStripMenuItem_Click;
            // 
            // tileSelectionToolStripMenuItem
            // 
            tileSelectionToolStripMenuItem.Name = "tileSelectionToolStripMenuItem";
            tileSelectionToolStripMenuItem.Size = new System.Drawing.Size(339, 44);
            tileSelectionToolStripMenuItem.Text = "Tile Selection (T)";
            tileSelectionToolStripMenuItem.Click += tileSelectionToolStripMenuItem_Click;
            // 
            // shapesToolStripMenuItem
            // 
            shapesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { gliderToolStripMenuItem, lwssToolStripMenuItem, blinkerToolStripMenuItem, toadToolStripMenuItem, beaconToolStripMenuItem, pulsarToolStripMenuItem, toolStripSeparator1, rPentominoToolStripMenuItem, acornToolStripMenuItem, diehardToolStripMenuItem, toolStripSeparator2, gosperGliderGunToolStripMenuItem });
            shapesToolStripMenuItem.Name = "shapesToolStripMenuItem";
            shapesToolStripMenuItem.Size = new System.Drawing.Size(339, 44);
            shapesToolStripMenuItem.Text = "Shapes...";
            // 
            // gliderToolStripMenuItem
            // 
            gliderToolStripMenuItem.Name = "gliderToolStripMenuItem";
            gliderToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            gliderToolStripMenuItem.Text = "Glider";
            gliderToolStripMenuItem.Click += gliderToolStripMenuItem_Click;
            // 
            // lwssToolStripMenuItem
            // 
            lwssToolStripMenuItem.Name = "lwssToolStripMenuItem";
            lwssToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            lwssToolStripMenuItem.Text = "Lightweight Spaceship (LWSS)";
            lwssToolStripMenuItem.Click += lwssToolStripMenuItem_Click;
            // 
            // blinkerToolStripMenuItem
            // 
            blinkerToolStripMenuItem.Name = "blinkerToolStripMenuItem";
            blinkerToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            blinkerToolStripMenuItem.Text = "Blinker";
            blinkerToolStripMenuItem.Click += blinkerToolStripMenuItem_Click;
            // 
            // toadToolStripMenuItem
            // 
            toadToolStripMenuItem.Name = "toadToolStripMenuItem";
            toadToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            toadToolStripMenuItem.Text = "Toad";
            toadToolStripMenuItem.Click += toadToolStripMenuItem_Click;
            // 
            // beaconToolStripMenuItem
            // 
            beaconToolStripMenuItem.Name = "beaconToolStripMenuItem";
            beaconToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            beaconToolStripMenuItem.Text = "Beacon";
            beaconToolStripMenuItem.Click += beaconToolStripMenuItem_Click;
            // 
            // pulsarToolStripMenuItem
            // 
            pulsarToolStripMenuItem.Name = "pulsarToolStripMenuItem";
            pulsarToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            pulsarToolStripMenuItem.Text = "Pulsar";
            pulsarToolStripMenuItem.Click += pulsarToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(462, 6);
            // 
            // rPentominoToolStripMenuItem
            // 
            rPentominoToolStripMenuItem.Name = "rPentominoToolStripMenuItem";
            rPentominoToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            rPentominoToolStripMenuItem.Text = "R-Pentomino";
            rPentominoToolStripMenuItem.Click += rPentominoToolStripMenuItem_Click;
            // 
            // acornToolStripMenuItem
            // 
            acornToolStripMenuItem.Name = "acornToolStripMenuItem";
            acornToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            acornToolStripMenuItem.Text = "Acorn";
            acornToolStripMenuItem.Click += acornToolStripMenuItem_Click;
            // 
            // diehardToolStripMenuItem
            // 
            diehardToolStripMenuItem.Name = "diehardToolStripMenuItem";
            diehardToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            diehardToolStripMenuItem.Text = "Diehard";
            diehardToolStripMenuItem.Click += diehardToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(462, 6);
            // 
            // gosperGliderGunToolStripMenuItem
            // 
            gosperGliderGunToolStripMenuItem.Name = "gosperGliderGunToolStripMenuItem";
            gosperGliderGunToolStripMenuItem.Size = new System.Drawing.Size(465, 44);
            gosperGliderGunToolStripMenuItem.Text = "Gosper Glider Gun";
            gosperGliderGunToolStripMenuItem.Click += gosperGliderGunToolStripMenuItem_Click;
            // 
            // tilingsToolStripMenuItem
            // 
            tilingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { chessboardToolStripMenuItem, brickPatternToolStripMenuItem, diagonalStripesToolStripMenuItem, doubleDiagonalToolStripMenuItem, concentricRectanglesToolStripMenuItem });
            tilingsToolStripMenuItem.Name = "tilingsToolStripMenuItem";
            tilingsToolStripMenuItem.Size = new System.Drawing.Size(339, 44);
            tilingsToolStripMenuItem.Text = "Tilings...";
            // 
            // chessboardToolStripMenuItem
            // 
            chessboardToolStripMenuItem.Name = "chessboardToolStripMenuItem";
            chessboardToolStripMenuItem.Size = new System.Drawing.Size(382, 44);
            chessboardToolStripMenuItem.Text = "Chessboard (5x5)";
            chessboardToolStripMenuItem.Click += chessboardToolStripMenuItem_Click;
            // 
            // brickPatternToolStripMenuItem
            // 
            brickPatternToolStripMenuItem.Name = "brickPatternToolStripMenuItem";
            brickPatternToolStripMenuItem.Size = new System.Drawing.Size(382, 44);
            brickPatternToolStripMenuItem.Text = "Ladder Brick";
            brickPatternToolStripMenuItem.Click += brickPatternToolStripMenuItem_Click;
            // 
            // diagonalStripesToolStripMenuItem
            // 
            diagonalStripesToolStripMenuItem.Name = "diagonalStripesToolStripMenuItem";
            diagonalStripesToolStripMenuItem.Size = new System.Drawing.Size(382, 44);
            diagonalStripesToolStripMenuItem.Text = "Diagonal Stripes";
            diagonalStripesToolStripMenuItem.Click += diagonalStripesToolStripMenuItem_Click;
            // 
            // doubleDiagonalToolStripMenuItem
            // 
            doubleDiagonalToolStripMenuItem.Name = "doubleDiagonalToolStripMenuItem";
            doubleDiagonalToolStripMenuItem.Size = new System.Drawing.Size(382, 44);
            doubleDiagonalToolStripMenuItem.Text = "Double Diagonal";
            doubleDiagonalToolStripMenuItem.Click += doubleDiagonalToolStripMenuItem_Click;
            // 
            // concentricRectanglesToolStripMenuItem
            // 
            concentricRectanglesToolStripMenuItem.Name = "concentricRectanglesToolStripMenuItem";
            concentricRectanglesToolStripMenuItem.Size = new System.Drawing.Size(382, 44);
            concentricRectanglesToolStripMenuItem.Text = "Concentric Rectangles";
            concentricRectanglesToolStripMenuItem.Click += concentricRectanglesToolStripMenuItem_Click;
            // 
            // colorModeComboBox
            // 
            colorModeComboBox.DropDownWidth = 450;
            colorModeComboBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            colorModeComboBox.Name = "colorModeComboBox";
            colorModeComboBox.Size = new System.Drawing.Size(450, 58);
            colorModeComboBox.SelectedIndexChanged += colorModeComboBox_SelectedIndexChanged;
            // 
            // toolStripIterationsTextbox
            // 
            toolStripIterationsTextbox.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold);
            toolStripIterationsTextbox.MaxLength = 200;
            toolStripIterationsTextbox.Name = "toolStripIterationsTextbox";
            toolStripIterationsTextbox.Size = new System.Drawing.Size(275, 58);
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripIterationsTextbox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripAliveCountBox
            // 
            toolStripAliveCountBox.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold);
            toolStripAliveCountBox.Name = "toolStripAliveCountBox";
            toolStripAliveCountBox.Size = new System.Drawing.Size(250, 58);
            toolStripAliveCountBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripModeIndicator
            // 
            toolStripModeIndicator.AutoSize = false;
            toolStripModeIndicator.BackColor = System.Drawing.Color.LightGreen;
            toolStripModeIndicator.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold);
            toolStripModeIndicator.Name = "toolStripModeIndicator";
            toolStripModeIndicator.ReadOnly = true;
            toolStripModeIndicator.Size = new System.Drawing.Size(350, 53);
            toolStripModeIndicator.Text = "Draw with mouse";
            toolStripModeIndicator.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripSpacebarComment
            // 
            toolStripSpacebarComment.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripSpacebarComment.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold);
            toolStripSpacebarComment.Name = "toolStripSpacebarComment";
            toolStripSpacebarComment.Size = new System.Drawing.Size(323, 58);
            toolStripSpacebarComment.Text = "Space = Start/Stop";
            toolStripSpacebarComment.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripTextBoxStatus
            // 
            toolStripTextBoxStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripTextBoxStatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            toolStripTextBoxStatus.ForeColor = System.Drawing.Color.Red;
            toolStripTextBoxStatus.Name = "toolStripTextBoxStatus";
            toolStripTextBoxStatus.Size = new System.Drawing.Size(156, 58);
            toolStripTextBoxStatus.Text = "Stopped";
            toolStripTextBoxStatus.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // myTimer
            // 
            myTimer.Tick += tmrClock_Tick;
            // 
            // squareModel
            // 
            squareModel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            squareModel.Location = new System.Drawing.Point(120, 167);
            squareModel.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            squareModel.Name = "squareModel";
            squareModel.Size = new System.Drawing.Size(32, 40);
            squareModel.TabIndex = 1;
            squareModel.TabStop = false;
            // 
            // squareModelAlive
            // 
            squareModelAlive.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            squareModelAlive.Location = new System.Drawing.Point(212, 167);
            squareModelAlive.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            squareModelAlive.Name = "squareModelAlive";
            squareModelAlive.Size = new System.Drawing.Size(32, 40);
            squareModelAlive.TabIndex = 2;
            squareModelAlive.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(2313, 1428);
            Controls.Add(squareModelAlive);
            Controls.Add(squareModel);
            Controls.Add(menuEtatVitesse);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            Name = "Form1";
            Text = "Game of Life";
            Load += Form1_Load;
            menuEtatVitesse.ResumeLayout(false);
            menuEtatVitesse.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)squareModel).EndInit();
            ((System.ComponentModel.ISupportInitialize)squareModelAlive).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuEtatVitesse;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vitesseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem middleSpeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rapideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileSelectionToolStripMenuItem;
        private System.Windows.Forms.Timer myTimer;
        private System.Windows.Forms.PictureBox squareModel;
        private System.Windows.Forms.PictureBox squareModelAlive;
        private System.Windows.Forms.ToolStripTextBox toolStripSpacebarComment;
        private System.Windows.Forms.ToolStripTextBox toolStripIterationsTextbox;
        private System.Windows.Forms.ToolStripTextBox toolStripAliveCountBox;
        private System.Windows.Forms.ToolStripTextBox toolStripModeIndicator;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxStatus;
        private System.Windows.Forms.ToolStripMenuItem exportPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox colorModeComboBox;
        private System.Windows.Forms.ToolStripMenuItem shapesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gliderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lwssToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blinkerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beaconToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pulsarToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem rPentominoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acornToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diehardToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem gosperGliderGunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tilingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chessboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brickPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diagonalStripesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doubleDiagonalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem concentricRectanglesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem transformToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotate90CWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotate90CCWToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem rotate180ToolStripMenuItem;
    }
}
