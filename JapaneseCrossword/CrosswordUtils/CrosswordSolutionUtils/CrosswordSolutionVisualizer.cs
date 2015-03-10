using System.Drawing;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils.Enums;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils.Interfaces;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils
{
    class CrosswordSolutionVisualizer : ICrosswordSolutionVisualizer
    {
        private readonly string outputFile;
        private const int CellWidth = 50;
        private const int CellHeight = 50;
        private const int BorderWidth = 1;
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

            using (var crosswordSolutionImage = new Bitmap(width, height))
            {
                using (var crosswordSolutionGraphics = Graphics.FromImage(crosswordSolutionImage))
                {
                    for (var x = 0; x < crosswordSolution.CrosswordCells.Count; ++x)
                    {
                        for (var y = 0; y < crosswordSolution.CrosswordCells[x].Count; ++y)
                        {
                            var cell = crosswordSolution.CrosswordCells[x][y];
                            DrawCell(cell, x, y, crosswordSolutionGraphics);
                        }
                    }
                }

                crosswordSolutionImage.Save(outputFile);
            }
        }

        private static void DrawCell(CrosswordSolutionCell solutionCell, int x, int y, Graphics crosswordSolutionGraphics)
        {
            SolidBrush rectangleBrush;
            var rectanglePen = new Pen(BorderColor, BorderWidth); 

            switch (solutionCell)
            {
                case CrosswordSolutionCell.Filled:
                {
                    rectangleBrush = new SolidBrush(FilledCellColor);
                    break;
                }
                case CrosswordSolutionCell.Empty:
                {
                    rectangleBrush = new SolidBrush(EmptyCellColor);
                    break;
                }
                default:
                {
                    rectangleBrush = new SolidBrush(UnclearCellColor);
                    break;
                }
            }

            crosswordSolutionGraphics.FillRectangle(rectangleBrush, y * CellWidth, x * CellHeight, CellWidth, CellHeight);
            crosswordSolutionGraphics.DrawRectangle(rectanglePen, y * CellWidth, x * CellHeight, CellWidth, CellHeight);
        }
    }
}
