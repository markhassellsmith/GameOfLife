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
            colorModeComboBox = new System.Windows.Forms.ToolStripComboBox();
            toolStripIterationsTextbox = new System.Windows.Forms.ToolStripTextBox();
            toolStripAliveCountBox = new System.Windows.Forms.ToolStripTextBox();
            toolStripModeIndicator = new System.Windows.Forms.ToolStripTextBox();
            toolStripSpacebarComment = new System.Windows.Forms.ToolStripTextBox();
            toolStripTextBoxStatus = new System.Windows.Forms.ToolStripTextBox();
            myTimer = new System.Windows.Forms.Timer(components);
            squareModel = new System.Windows.Forms.PictureBox();
            squareModelAlive = new System.Windows.Forms.PictureBox();
            tilingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            chessboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            brickPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            diagonalStripesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            doubleDiagonalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            concentricRectanglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuEtatVitesse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)squareModel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)squareModelAlive).BeginInit();
            SuspendLayout();
            // 
            // menuEtatVitesse
            // 
            menuEtatVitesse.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuEtatVitesse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, simulationToolStripMenuItem, vitesseToolStripMenuItem, patternToolStripMenuItem, colorModeComboBox, toolStripIterationsTextbox, toolStripAliveCountBox, toolStripModeIndicator, toolStripSpacebarComment, toolStripTextBoxStatus });
            menuEtatVitesse.Location = new System.Drawing.Point(0, 0);
            menuEtatVitesse.Name = "menuEtatVitesse";
            menuEtatVitesse.Padding = new System.Windows.Forms.Padding(16, 4, 0, 4);
            menuEtatVitesse.Size = new System.Drawing.Size(3066, 66);
            menuEtatVitesse.TabIndex = 0;
            menuEtatVitesse.Text = "menuStrip1";
            menuEtatVitesse.ItemClicked += menuEtatVitesse_ItemClicked;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { resetToolStripMenuItem, clearGridToolStripMenuItem, exportPatternToolStripMenuItem, importPatternToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(99, 58);
            fileToolStripMenuItem.Text = "File";
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new System.Drawing.Size(433, 60);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // clearGridToolStripMenuItem
            // 
            clearGridToolStripMenuItem.Name = "clearGridToolStripMenuItem";
            clearGridToolStripMenuItem.Size = new System.Drawing.Size(433, 60);
            clearGridToolStripMenuItem.Text = "Clear the Grid";
            clearGridToolStripMenuItem.Click += clearGridToolStripMenuItem_Click;
            // 
            // exportPatternToolStripMenuItem
            // 
            exportPatternToolStripMenuItem.Name = "exportPatternToolStripMenuItem";
            exportPatternToolStripMenuItem.Size = new System.Drawing.Size(433, 60);
            exportPatternToolStripMenuItem.Text = "Export Pattern ...";
            exportPatternToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            exportPatternToolStripMenuItem.Click += exportPatternToolStripMenuItem_Click;
            // 
            // importPatternToolStripMenuItem
            // 
            importPatternToolStripMenuItem.Name = "importPatternToolStripMenuItem";
            importPatternToolStripMenuItem.Size = new System.Drawing.Size(433, 60);
            importPatternToolStripMenuItem.Text = "Import Pattern..";
            importPatternToolStripMenuItem.Click += importPatternToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            exitToolStripMenuItem.Size = new System.Drawing.Size(433, 60);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // simulationToolStripMenuItem
            // 
            simulationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { startToolStripMenuItem, stopToolStripMenuItem });
            simulationToolStripMenuItem.Name = "simulationToolStripMenuItem";
            simulationToolStripMenuItem.Size = new System.Drawing.Size(220, 58);
            simulationToolStripMenuItem.Text = "Simulation";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            startToolStripMenuItem.Size = new System.Drawing.Size(240, 60);
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Click += runToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            stopToolStripMenuItem.Size = new System.Drawing.Size(240, 60);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // vitesseToolStripMenuItem
            // 
            vitesseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { lentToolStripMenuItem, middleSpeedToolStripMenuItem, rapideToolStripMenuItem });
            vitesseToolStripMenuItem.Name = "vitesseToolStripMenuItem";
            vitesseToolStripMenuItem.Size = new System.Drawing.Size(146, 58);
            vitesseToolStripMenuItem.Text = "Speed";
            // 
            // lentToolStripMenuItem
            // 
            lentToolStripMenuItem.Name = "lentToolStripMenuItem";
            lentToolStripMenuItem.Size = new System.Drawing.Size(303, 60);
            lentToolStripMenuItem.Text = "Slow";
            lentToolStripMenuItem.Click += slowToolStripMenuItem_Click;
            // 
            // middleSpeedToolStripMenuItem
            // 
            middleSpeedToolStripMenuItem.Name = "middleSpeedToolStripMenuItem";
            middleSpeedToolStripMenuItem.Size = new System.Drawing.Size(303, 60);
            middleSpeedToolStripMenuItem.Text = "Medium";
            middleSpeedToolStripMenuItem.Click += normalSpeedToolStripMenuItem_Click;
            // 
            // rapideToolStripMenuItem
            // 
            rapideToolStripMenuItem.Name = "rapideToolStripMenuItem";
            rapideToolStripMenuItem.Size = new System.Drawing.Size(303, 60);
            rapideToolStripMenuItem.Text = "Quick";
            rapideToolStripMenuItem.Click += quickToolStripMenuItem_Click;
            // 
            // patternToolStripMenuItem
            // 
            patternToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { randomToolStripMenuItem, tileSelectionToolStripMenuItem, shapesToolStripMenuItem, tilingsToolStripMenuItem });
            patternToolStripMenuItem.Name = "patternToolStripMenuItem";
            patternToolStripMenuItem.Size = new System.Drawing.Size(162, 58);
            patternToolStripMenuItem.Text = "Pattern";
            // 
            // randomToolStripMenuItem
            // 
            randomToolStripMenuItem.Checked = true;
            randomToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            randomToolStripMenuItem.Size = new System.Drawing.Size(381, 60);
            randomToolStripMenuItem.Text = "Random";
            randomToolStripMenuItem.Click += randomToolStripMenuItem_Click;
            // 
            // tileSelectionToolStripMenuItem
            // 
            tileSelectionToolStripMenuItem.Name = "tileSelectionToolStripMenuItem";
            tileSelectionToolStripMenuItem.Size = new System.Drawing.Size(381, 60);
            tileSelectionToolStripMenuItem.Text = "Tile Selection... (T)";
            tileSelectionToolStripMenuItem.Click += tileSelectionToolStripMenuItem_Click;
            // 
            // shapesToolStripMenuItem
            // 
            shapesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { gliderToolStripMenuItem, lwssToolStripMenuItem, blinkerToolStripMenuItem, toadToolStripMenuItem, beaconToolStripMenuItem, pulsarToolStripMenuItem, toolStripSeparator1, rPentominoToolStripMenuItem, acornToolStripMenuItem, diehardToolStripMenuItem, toolStripSeparator2, gosperGliderGunToolStripMenuItem });
            shapesToolStripMenuItem.Name = "shapesToolStripMenuItem";
            shapesToolStripMenuItem.Size = new System.Drawing.Size(381, 60);
            shapesToolStripMenuItem.Text = "Shapes...";
            // 
            // gliderToolStripMenuItem
            // 
            gliderToolStripMenuItem.Name = "gliderToolStripMenuItem";
            gliderToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            gliderToolStripMenuItem.Text = "Glider";
            gliderToolStripMenuItem.Click += gliderToolStripMenuItem_Click;
            // 
            // lwssToolStripMenuItem
            // 
            lwssToolStripMenuItem.Name = "lwssToolStripMenuItem";
            lwssToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            lwssToolStripMenuItem.Text = "Lightweight Spaceship (LWSS)";
            lwssToolStripMenuItem.Click += lwssToolStripMenuItem_Click;
            // 
            // blinkerToolStripMenuItem
            // 
            blinkerToolStripMenuItem.Name = "blinkerToolStripMenuItem";
            blinkerToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            blinkerToolStripMenuItem.Text = "Blinker";
            blinkerToolStripMenuItem.Click += blinkerToolStripMenuItem_Click;
            // 
            // toadToolStripMenuItem
            // 
            toadToolStripMenuItem.Name = "toadToolStripMenuItem";
            toadToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            toadToolStripMenuItem.Text = "Toad";
            toadToolStripMenuItem.Click += toadToolStripMenuItem_Click;
            // 
            // beaconToolStripMenuItem
            // 
            beaconToolStripMenuItem.Name = "beaconToolStripMenuItem";
            beaconToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            beaconToolStripMenuItem.Text = "Beacon";
            beaconToolStripMenuItem.Click += beaconToolStripMenuItem_Click;
            // 
            // pulsarToolStripMenuItem
            // 
            pulsarToolStripMenuItem.Name = "pulsarToolStripMenuItem";
            pulsarToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            pulsarToolStripMenuItem.Text = "Pulsar";
            pulsarToolStripMenuItem.Click += pulsarToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(659, 6);
            // 
            // rPentominoToolStripMenuItem
            // 
            rPentominoToolStripMenuItem.Name = "rPentominoToolStripMenuItem";
            rPentominoToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            rPentominoToolStripMenuItem.Text = "R-Pentomino";
            rPentominoToolStripMenuItem.Click += rPentominoToolStripMenuItem_Click;
            // 
            // acornToolStripMenuItem
            // 
            acornToolStripMenuItem.Name = "acornToolStripMenuItem";
            acornToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            acornToolStripMenuItem.Text = "Acorn";
            acornToolStripMenuItem.Click += acornToolStripMenuItem_Click;
            // 
            // diehardToolStripMenuItem
            // 
            diehardToolStripMenuItem.Name = "diehardToolStripMenuItem";
            diehardToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            diehardToolStripMenuItem.Text = "Diehard";
            diehardToolStripMenuItem.Click += diehardToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(659, 6);
            // 
            // gosperGliderGunToolStripMenuItem
            // 
            gosperGliderGunToolStripMenuItem.Name = "gosperGliderGunToolStripMenuItem";
            gosperGliderGunToolStripMenuItem.Size = new System.Drawing.Size(662, 60);
            gosperGliderGunToolStripMenuItem.Text = "Gosper Glider Gun";
            gosperGliderGunToolStripMenuItem.Click += gosperGliderGunToolStripMenuItem_Click;
            // 
            // colorModeComboBox
            // 
            colorModeComboBox.DropDownWidth = 450;
            colorModeComboBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            colorModeComboBox.Name = "colorModeComboBox";
            colorModeComboBox.Size = new System.Drawing.Size(400, 58);
            colorModeComboBox.SelectedIndexChanged += colorModeComboBox_SelectedIndexChanged;
            // 
            // toolStripIterationsTextbox
            // 
            toolStripIterationsTextbox.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            toolStripIterationsTextbox.MaxLength = 200;
            toolStripIterationsTextbox.Name = "toolStripIterationsTextbox";
            toolStripIterationsTextbox.Size = new System.Drawing.Size(250, 58);
            toolStripIterationsTextbox.Text = "Tick = 0";
            toolStripIterationsTextbox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripAliveCountBox
            // 
            toolStripAliveCountBox.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            toolStripAliveCountBox.Name = "toolStripAliveCountBox";
            toolStripAliveCountBox.Size = new System.Drawing.Size(245, 58);
            toolStripAliveCountBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripModeIndicator
            // 
            toolStripModeIndicator.AutoSize = false;
            toolStripModeIndicator.BackColor = System.Drawing.Color.LightGreen;
            toolStripModeIndicator.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            toolStripModeIndicator.Name = "toolStripModeIndicator";
            toolStripModeIndicator.ReadOnly = true;
            toolStripModeIndicator.Size = new System.Drawing.Size(320, 58);
            toolStripModeIndicator.Text = "Mode: Drawing";
            toolStripModeIndicator.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripSpacebarComment
            // 
            toolStripSpacebarComment.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripSpacebarComment.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            toolStripSpacebarComment.Name = "toolStripSpacebarComment";
            toolStripSpacebarComment.Size = new System.Drawing.Size(520, 58);
            toolStripSpacebarComment.Text = "Spacebar=Start/Stop";
            toolStripSpacebarComment.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // toolStripTextBoxStatus
            // 
            toolStripTextBoxStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripTextBoxStatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            toolStripTextBoxStatus.ForeColor = System.Drawing.Color.Red;
            toolStripTextBoxStatus.Name = "toolStripTextBoxStatus";
            toolStripTextBoxStatus.Size = new System.Drawing.Size(250, 58);
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
            squareModel.Location = new System.Drawing.Point(194, 266);
            squareModel.Margin = new System.Windows.Forms.Padding(7, 11, 7, 11);
            squareModel.Name = "squareModel";
            squareModel.Size = new System.Drawing.Size(51, 64);
            squareModel.TabIndex = 1;
            squareModel.TabStop = false;
            // 
            // squareModelAlive
            // 
            squareModelAlive.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            squareModelAlive.Location = new System.Drawing.Point(343, 266);
            squareModelAlive.Margin = new System.Windows.Forms.Padding(7, 11, 7, 11);
            squareModelAlive.Name = "squareModelAlive";
            squareModelAlive.Size = new System.Drawing.Size(51, 64);
            squareModelAlive.TabIndex = 2;
            squareModelAlive.TabStop = false;
            // 
            // tilingsToolStripMenuItem
            // 
            tilingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { 
    chessboardToolStripMenuItem, 
    brickPatternToolStripMenuItem, 
    diagonalStripesToolStripMenuItem,
    doubleDiagonalToolStripMenuItem,
    concentricRectanglesToolStripMenuItem 
});
            tilingsToolStripMenuItem.Name = "tilingsToolStripMenuItem";
            tilingsToolStripMenuItem.Size = new System.Drawing.Size(381, 60);
            tilingsToolStripMenuItem.Text = "Tilings...";
            // 
            // chessboardToolStripMenuItem
            // 
            chessboardToolStripMenuItem.Name = "chessboardToolStripMenuItem";
            chessboardToolStripMenuItem.Size = new System.Drawing.Size(450, 60);
            chessboardToolStripMenuItem.Text = "Chessboard (5x5)";
            chessboardToolStripMenuItem.Click += chessboardToolStripMenuItem_Click;
            // 
            // brickPatternToolStripMenuItem
            // 
            brickPatternToolStripMenuItem.Name = "brickPatternToolStripMenuItem";
            brickPatternToolStripMenuItem.Size = new System.Drawing.Size(450, 60);
            brickPatternToolStripMenuItem.Text = "Ladder Brick";
            brickPatternToolStripMenuItem.Click += brickPatternToolStripMenuItem_Click;
            // 
            // diagonalStripesToolStripMenuItem
            // 
            diagonalStripesToolStripMenuItem.Name = "diagonalStripesToolStripMenuItem";
            diagonalStripesToolStripMenuItem.Size = new System.Drawing.Size(450, 60);
            diagonalStripesToolStripMenuItem.Text = "Diagonal Stripes";
            diagonalStripesToolStripMenuItem.Click += diagonalStripesToolStripMenuItem_Click;
            // 
            // doubleDiagonalToolStripMenuItem
            // 
            doubleDiagonalToolStripMenuItem.Name = "doubleDiagonalToolStripMenuItem";
            doubleDiagonalToolStripMenuItem.Size = new System.Drawing.Size(450, 60);
            doubleDiagonalToolStripMenuItem.Text = "Double Diagonal";
            doubleDiagonalToolStripMenuItem.Click += doubleDiagonalToolStripMenuItem_Click;
            // 
            // concentricRectanglesToolStripMenuItem
            // 
            concentricRectanglesToolStripMenuItem.Name = "concentricRectanglesToolStripMenuItem";
            concentricRectanglesToolStripMenuItem.Size = new System.Drawing.Size(450, 60);
            concentricRectanglesToolStripMenuItem.Text = "Concentric Rectangles";
            concentricRectanglesToolStripMenuItem.Click += concentricRectanglesToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(21F, 51F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(3066, 1316);
            Controls.Add(squareModelAlive);
            Controls.Add(squareModel);
            Controls.Add(menuEtatVitesse);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(7, 11, 7, 11);
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
    }
}
