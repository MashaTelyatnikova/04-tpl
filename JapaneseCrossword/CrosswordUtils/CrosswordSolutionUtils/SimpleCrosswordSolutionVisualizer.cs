using System;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils
{
    public class SimpleCrosswordSolutionVisualizer : ICrosswordSolutionVisualizer
    {
        public void Visualize(CrosswordSolution crosswordSolution)
        {
            foreach (var line in crosswordSolution.CrosswordCells)
            {
                foreach (var cell in line)
                {
                    Console.Write(GetCharAtCell(cell));
                }

                Console.WriteLine();
            }
        }

        private static char GetCharAtCell(CrosswordSolutionCell solutionCell)
        {
            switch (solutionCell)
            {
                case CrosswordSolutionCell.Empty:
                    {
                        return '.';
                    }
                case CrosswordSolutionCell.Filled:
                    {
                        return '*';
                    }
                default:
                    {
                        throw new Exception();
                    }
            }
        }
    }
}
