using System.Collections.Generic;
using JapaneseCrossword.CrosswordUtils.CrosswordSolution;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils
{
    public class CrosswordSolution
    {
        public List<List<CrosswordCell>> CrosswordCells { get; private set; }
        public SolutionStatus Status { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public CrosswordSolution(List<List<CrosswordCell>> crosswordCells, SolutionStatus status)
        {
            CrosswordCells = crosswordCells;
            Status = status;
            Width = CrosswordCells.Count;
            Height = Width == 0 ? 0 : CrosswordCells[0].Count;
        }
    }
}
