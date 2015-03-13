using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordSolutionUtils;
using MoreLinq;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public abstract class MyCrosswordSolver : ICrosswordSolver
    {
        private Crossword crossword;
        private CrosswordField crosswordField;
        protected Dictionary<LineType, List<bool>> LinesForUpdatingAtType;

        public CrosswordSolution Solve(Crossword crosswordParam)
        {
            InitCurrentState(crosswordParam);

            while (ExistsLinesForUpdating())
            {
                var rowsResult = UpdateLines(LineType.Row);
                var columnResult = UpdateLines(LineType.Column);

                if (!rowsResult || !columnResult)
                {
                    return new CrosswordSolution(Enumerable.Empty<List<CrosswordCell>>().ToList(),
                       SolutionStatus.IncorrectCrossword);
                }
            }

            return new CrosswordSolution(crosswordField.ToMatrix(),
                crosswordField.IsFilled() ? SolutionStatus.Solved : SolutionStatus.PartiallySolved);
        }

        private void InitCurrentState(Crossword crosswordParam)
        {
            crossword = crosswordParam;
            crosswordField = new CrosswordField(crossword.Width, crossword.Height);

            LinesForUpdatingAtType = new Dictionary<LineType, List<bool>>();
            LinesForUpdatingAtType[LineType.Row] = Enumerable.Repeat(true, crossword.Height).ToList();
            LinesForUpdatingAtType[LineType.Column] = Enumerable.Repeat(true, crossword.Width).ToList();

        }

        private bool ExistsLinesForUpdating()
        {
            return LinesForUpdatingAtType[LineType.Row].Any(i => i) ||
                   LinesForUpdatingAtType[LineType.Column].Any(i => i);
        }

        protected abstract bool UpdateLines(LineType type);
        
        protected void UpdateLine(LineType type, int lineNumber, List<CrosswordCell> cells)
        {
            var invertedLineType = type == LineType.Row ? LineType.Column : LineType.Row;

            var oldCells = GetLineCells(type, lineNumber).Select((cell, column) => Tuple.Create(cell, column)).ToList();
            var newCells = cells.Select((cell, column) => Tuple.Create(cell, column)).ToList();

            newCells.Except(oldCells).ForEach(tuple =>
            {
                LinesForUpdatingAtType[invertedLineType][tuple.Item2] = true;
                if (type == LineType.Row)
                    crosswordField[lineNumber, tuple.Item2] = newCells[tuple.Item2].Item1;
                else
                    crosswordField[tuple.Item2, lineNumber] = newCells[tuple.Item2].Item1;
            });
        }

        protected List<CrosswordCell> GetLineCells(LineType type, int number)
        {
            return type == LineType.Row
                ? crosswordField.GetRowCells(number)
                : crosswordField.GetColumnCells(number);
        }

        protected CrosswordLine GetCrosswordLine(LineType type, int number)
        {
            return type == LineType.Row ? crossword.Rows[number] : crossword.Columns[number];
        }
    }
}
