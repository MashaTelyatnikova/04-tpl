using System;
using System.Linq;
using JapaneseCrossword.CrosswordSolverUtils;
using MoreLinq;

namespace JapaneseCrossword.CrosswordSolutionUtils
{
    public class CrosswordSolutionTextVisualizer : ICrosswordSolutionVisualizer<string>
    {
        public string Visualize(CrosswordSolution crosswordSolution)
        {
            return  crosswordSolution
                        .CrosswordCells
                        .Select(cells => cells.Select(GetCharAtCell).ToDelimitedString(""))
                        .ToDelimitedString("\n");
        }

        private static char GetCharAtCell(CrosswordCell solutionCell)
        {
            switch (solutionCell)
            {
                case CrosswordCell.Empty:
                    {
                        return '.';
                    }
                case CrosswordCell.Filled:
                    {
                        return '*';
                    }
                case CrosswordCell.Unclear:
                    {
                        return '?';
                    }
                default:
                    {
                        throw new Exception();
                    }
            }
        }
    }
}
