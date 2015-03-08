using System.Drawing;
using JapaneseCrossword.CrosswordUtils.CrosswordSolution;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils
{
    class CrosswordSolutionVisualizer : ICrosswordSolutionVisualizer
    {
        private string outputFile;
        private const int CellWidth = 50;
        private const int CellHeight = 50;
        private static readonly Color BorderColor = Color.FromArgb(156, 156, 156);
        private static readonly Color FilledCellColor = Color.Black;
        private static readonly Color EmptyCellColor = Color.White;
        private static readonly Color UnclearCellColor = Color.FromArgb(156, 156, 156);

        public CrosswordSolutionVisualizer(string outputFile)
        {
            this.outputFile = outputFile;
        }

        public void Visualize(CrosswordSolution crosswordSolution)
        {
            var width = crosswordSolution.Width * CellWidth;
            var height = crosswordSolution.Height * CellHeight;

            using (var bitmap = new Bitmap(width, height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    for (var i = 0; i < crosswordSolution.CrosswordCells.Count; ++i)
                    {
                        for (var j = 0; j < crosswordSolution.CrosswordCells[i].Count; ++j)
                        {
                            var cell = crosswordSolution.CrosswordCells[i][j];
                            DrawCell(cell, i, j, graphics);
                        }
                    }
                }

                bitmap.Save(outputFile);
            }
        }

        private static void DrawCell(CrosswordCell cell, int i, int j, Graphics graphics)
        {
            if (cell == CrosswordCell.Filled)
            {

                graphics.FillRectangle(new SolidBrush(FilledCellColor), j * CellWidth, i * CellHeight, CellWidth, CellHeight);
                graphics.DrawRectangle(new Pen(BorderColor, 1), j * CellWidth, i * CellHeight, CellWidth, CellHeight);
            }
            else if (cell == CrosswordCell.Empty)
            {
                graphics.FillRectangle(new SolidBrush(EmptyCellColor), j*CellWidth, i*CellHeight, CellWidth, CellHeight);
                graphics.DrawRectangle(new Pen(BorderColor, 1), j*CellWidth, i*CellHeight, CellWidth, CellHeight);
            }
            else
            {
                graphics.FillRectangle(new SolidBrush(UnclearCellColor), j * CellWidth, i * CellHeight, CellWidth, CellHeight);
                graphics.DrawRectangle(new Pen(BorderColor, 1), j * CellWidth, i * CellHeight, CellWidth, CellHeight);
            }
        }
    }
}
