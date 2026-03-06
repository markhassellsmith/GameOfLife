using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    /// <summary>
    /// Dialog for previewing a pattern before loading it into the game.
    /// </summary>
    public class PatternPreviewDialog : Form
    {
        private Pattern pattern;
        private PictureBox previewPictureBox;
        private Label nameLabel;
        private Label descriptionLabel;
        private Label sizeLabel;
        private Button loadButton;
        private Button cancelButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternPreviewDialog"/> class.
        /// </summary>
        /// <param name="pattern">The pattern to preview.</param>
        public PatternPreviewDialog(Pattern pattern)
        {
            this.pattern = pattern;
            InitializeComponents();
            RenderPreview();
        }

        private void InitializeComponents()
        {
            // Form setup
            this.Text = "Pattern Preview";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Name label
            nameLabel = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(460, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = $"Name: {pattern.Name}"
            };
            this.Controls.Add(nameLabel);

            // Size label
            sizeLabel = new Label
            {
                Location = new Point(20, 55),
                Size = new Size(460, 25),
                Text = $"Size: {pattern.Width} × {pattern.Height}  |  Rule: {pattern.Rule}"
            };
            this.Controls.Add(sizeLabel);

            // Description label
            descriptionLabel = new Label
            {
                Location = new Point(20, 85),
                Size = new Size(460, 60),
                Text = string.IsNullOrEmpty(pattern.Description) ? "No description" : pattern.Description
            };
            this.Controls.Add(descriptionLabel);

            // Preview picture box
            previewPictureBox = new PictureBox
            {
                Location = new Point(20, 155),
                Size = new Size(460, 340),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
            this.Controls.Add(previewPictureBox);

            // Load button
            loadButton = new Button
            {
                Location = new Point(280, 515),
                Size = new Size(90, 35),
                Text = "Load",
                DialogResult = DialogResult.OK
            };
            this.Controls.Add(loadButton);
            this.AcceptButton = loadButton;

            // Cancel button
            cancelButton = new Button
            {
                Location = new Point(380, 515),
                Size = new Size(90, 35),
                Text = "Cancel",
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(cancelButton);
            this.CancelButton = cancelButton;
        }

        private void RenderPreview()
        {
            // Calculate cell size for preview
            int maxPreviewWidth = 440;
            int maxPreviewHeight = 320;

            int cellSize = Math.Min(maxPreviewWidth / pattern.Width, maxPreviewHeight / pattern.Height);
            cellSize = Math.Max(2, Math.Min(cellSize, 20)); // Between 2 and 20 pixels

            int bitmapWidth = pattern.Width * cellSize;
            int bitmapHeight = pattern.Height * cellSize;

            Bitmap preview = new Bitmap(bitmapWidth, bitmapHeight);

            using (Graphics g = Graphics.FromImage(preview))
            {
                g.Clear(Color.LightGray);

                for (int x = 0; x < pattern.Width; x++)
                {
                    for (int y = 0; y < pattern.Height; y++)
                    {
                        if (pattern.Cells[x, y])
                        {
                            g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                        }
                    }
                }
            }

            previewPictureBox.Image = preview;
        }
    }
}
