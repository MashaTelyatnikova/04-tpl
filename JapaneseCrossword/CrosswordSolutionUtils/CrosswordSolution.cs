using System;
using System.Collections.Generic;
using JapaneseCrossword.CrosswordSolverUtils;

namespace JapaneseCrossword.CrosswordSolutionUtils
{
    public class CrosswordSolution
    {
        public List<List<CrosswordCell>> CrosswordCells { get; private set; }
        public CrosswordSolutionStatus Status { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public CrosswordSolution(List<List<CrosswordCell>> crosswordCells, CrosswordSolutionStatus status)
        {
            if (crosswordCells == null)
            {
                throw new ArgumentNullException();
            }

            CrosswordCells = crosswordCells;
            Status = status;

            Height = CrosswordCells.Count;
            Width = Height == 0 ? 0 : CrosswordCells[0].Count;
        }
    }
}
