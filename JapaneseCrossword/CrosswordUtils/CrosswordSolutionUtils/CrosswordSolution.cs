using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils
{
    public class CrosswordSolution
    {
        public List<List<CrosswordSolutionCell>> CrosswordCells { get; private set; }
        public SolutionStatus Status { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public CrosswordSolution(List<List<CrosswordSolutionCell>> crosswordCells, SolutionStatus status)
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

        public override bool Equals(object obj)
        {
            var other = (CrosswordSolution) obj;
            return Width == other.Width && Height == other.Height && Status == other.Status && CrosswordCells.Count == other.CrosswordCells.Count &&
                CrosswordCells.Select((line, index) => line.SequenceEqual(other.CrosswordCells[index])).All(x => x);
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var line in CrosswordCells)
            {
                foreach (var cell in line)
                {
                    str.Append(GetCharAtCell(cell));
                }

                str.Append("\n");
            }
            return str.ToString().Trim();
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
                    case CrosswordSolutionCell.Unclear:
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
