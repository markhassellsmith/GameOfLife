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
            toolStripIterationsTextbox = new System.Windows.Forms.ToolStripTextBox();
            toolStripAliveCountBox = new System.Windows.Forms.ToolStripTextBox();
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
            menuEtatVitesse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, simulationToolStripMenuItem, vitesseToolStripMenuItem, patternToolStripMenuItem, toolStripIterationsTextbox, toolStripAliveCountBox, toolStripSpacebarComment, toolStripTextBoxStatus });
            menuEtatVitesse.Location = new System.Drawing.Point(0, 0);
            menuEtatVitesse.Name = "menuEtatVitesse";
            menuEtatVitesse.Padding = new System.Windows.Forms.Padding(16, 4, 0, 4);
            menuEtatVitesse.Size = new System.Drawing.Size(2146, 66);
            menuEtatVitesse.TabIndex = 0;
            menuEtatVitesse.Text = "menuStrip1";
            menuEtatVitesse.ItemClicked += menuEtatVitesse_ItemClicked;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { resetToolStripMenuItem, exportPatternToolStripMenuItem, importPatternToolStripMenuItem, exitToolStripMenuItem });
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
            startToolStripMenuItem.Size = new System.Drawing.Size(240, 60);
            startToolStripMenuItem.Text = "Start";
            startToolStripMenuItem.Click += runToolStripMenuItem_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
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
            patternToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { randomToolStripMenuItem });
            patternToolStripMenuItem.Name = "patternToolStripMenuItem";
            patternToolStripMenuItem.Size = new System.Drawing.Size(162, 58);
            patternToolStripMenuItem.Text = "Pattern";
            // 
            // randomToolStripMenuItem
            // 
            randomToolStripMenuItem.Checked = true;
            randomToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            randomToolStripMenuItem.Size = new System.Drawing.Size(304, 60);
            randomToolStripMenuItem.Text = "Random";
            randomToolStripMenuItem.Click += randomToolStripMenuItem_Click;
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
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(21F, 51F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(2146, 1318);
            Controls.Add(squareModelAlive);
            Controls.Add(squareModel);
            Controls.Add(menuEtatVitesse);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
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
        private System.Windows.Forms.Timer myTimer;
        private System.Windows.Forms.PictureBox squareModel;
        private System.Windows.Forms.PictureBox squareModelAlive;
        private System.Windows.Forms.ToolStripTextBox toolStripSpacebarComment;
        private System.Windows.Forms.ToolStripTextBox toolStripIterationsTextbox;
        private System.Windows.Forms.ToolStripTextBox toolStripAliveCountBox;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxStatus;
        private System.Windows.Forms.ToolStripMenuItem exportPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPatternToolStripMenuItem;
    }
}
