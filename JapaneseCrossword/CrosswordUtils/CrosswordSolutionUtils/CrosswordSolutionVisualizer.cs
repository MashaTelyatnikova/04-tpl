using System.Drawing;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils
{
    class CrosswordSolutionVisualizer : ICrosswordSolutionVisualizer
    {
        private readonly string outputFile;
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

        private static void DrawCell(CrosswordSolutionCell solutionCell, int i, int j, Graphics graphics)
        {
            SolidBrush rectangleBrush;
            var rectanglePen = new Pen(BorderColor, 1); 

            switch (solutionCell)
            {
                case CrosswordSolutionCell.Filled:
                    rectangleBrush= new SolidBrush(FilledCellColor);
                    break;
                case CrosswordSolutionCell.Empty:
                    rectangleBrush = new SolidBrush(EmptyCellColor);
                    break;
                default:
                    rectangleBrush = new SolidBrush(UnclearCellColor);
                    break;
            }

            graphics.FillRectangle(rectangleBrush, j * CellWidth, i * CellHeight, CellWidth, CellHeight);
            graphics.DrawRectangle(rectanglePen, j * CellWidth, i * CellHeight, CellWidth, CellHeight);
        }
    }
}
