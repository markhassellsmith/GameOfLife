using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TP14_JeudelaVie
{
    /// <summary>
    /// Handles rendering of the Game of Life grid to a bitmap.
    /// </summary>
    public class GridRenderer
    {
        private readonly Bitmap gridBitmap;
        private readonly PictureBox gridDisplay;
        private readonly PictureBox squareModel;
        private readonly int squarePerLine;
        private readonly int squarePerColumn;
        private readonly int squareSize;
        private readonly int cellSpacing;

        /// <summary>
        /// Initializes a new instance of the GridRenderer class.
        /// </summary>
        /// <param name="bitmap">The bitmap to render the grid onto.</param>
        /// <param name="display">The PictureBox control that displays the grid.</param>
        /// <param name="deadCellModel">The PictureBox used as a model for dead cell appearance.</param>
        /// <param name="gridWidth">The number of cells horizontally.</param>
        /// <param name="gridHeight">The number of cells vertically.</param>
        /// <param name="cellSize">The size of each cell in pixels.</param>
        /// <param name="spacing">The spacing between cells in pixels.</param>
        public GridRenderer(
            Bitmap bitmap,
            PictureBox display,
            PictureBox deadCellModel,
            int gridWidth,
            int gridHeight,
            int cellSize,
            int spacing)
        {
            gridBitmap = bitmap;
            gridDisplay = display;
            squareModel = deadCellModel;
            squarePerLine = gridWidth;
            squarePerColumn = gridHeight;
            squareSize = cellSize;
            cellSpacing = spacing;
        }

        /// <summary>
        /// Renders the entire grid.
        /// </summary>
        public void RenderGrid(bool[,] squaresState, Color[,] cellColor)
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

        /// <summary>
        /// Renders only the changed cells (dirty rendering for performance).
        /// </summary>
        public void RenderGridDirty(bool[,] squaresState, Color[,] cellColor, List<(int i, int j)> changedCells)
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
    }
}
