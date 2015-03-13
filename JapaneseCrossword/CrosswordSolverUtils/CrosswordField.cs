using System;
using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public class CrosswordField
    {
        private readonly List<List<CrosswordCell>> rows;
        private readonly List<List<CrosswordCell>> columns;

        public CrosswordField(int width, int height)
        {
            rows = Enumerable.Range(0, height)
                             .Select(i => Enumerable.Range(0, width).Select(j => CrosswordCell.Unclear).ToList())
                             .ToList();

            columns = Enumerable.Range(0, width)
                                .Select(i => Enumerable.Range(0, height).Select(j => CrosswordCell.Unclear).ToList())
                                .ToList();
        }

        public CrosswordCell this[int row, int column]
        {
            get { return rows[row][column]; }
            set
            {
                if (row < 0 || column < 0 || row >= rows.Count || column >= columns.Count)
                    throw new ArgumentOutOfRangeException();

                rows[row][column] = value;
                columns[column][row] = value;
            }
        }

        public List<CrosswordCell> GetColumnCells(int index)
        {
            return columns[index];
        }

        public List<CrosswordCell> GetRowCells(int index)
        {
            return rows[index];
        }

        public bool IsFilled()
        {
            return rows.All(row => !row.Contains(CrosswordCell.Unclear));
        }

        public List<List<CrosswordCell>> ToMatrix()
        {
            return rows;
        }
    }
}
