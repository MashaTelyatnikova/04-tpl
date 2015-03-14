using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordSolutionUtils;
using MoreLinq;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public abstract class CrosswordSolver : ICrosswordSolver
    {
        private Crossword crossword;
        private CrosswordField crosswordField;
        protected Dictionary<CrosswordLineType, List<bool>> LinesForUpdatingAtType;

        public CrosswordSolution Solve(Crossword crosswordParam)
        {
            InitCurrentState(crosswordParam);

            while (ExistsLinesForUpdating())
            {
                var rowsResult = UpdateLines(CrosswordLineType.Row);
                var columnsResult = UpdateLines(CrosswordLineType.Column);

                if (!rowsResult || !columnsResult)
                {
                    return new CrosswordSolution(Enumerable.Empty<List<CrosswordCell>>().ToList(),
                       CrosswordSolutionStatus.IncorrectCrossword);
                }
            }

            return new CrosswordSolution(crosswordField.ToMatrix(),
                crosswordField.IsFilled() ? CrosswordSolutionStatus.Solved : CrosswordSolutionStatus.PartiallySolved);
        }

        private void InitCurrentState(Crossword crosswordParam)
        {
            crossword = crosswordParam;
            crosswordField = new CrosswordField(crossword.Width, crossword.Height);

            LinesForUpdatingAtType = new Dictionary<CrosswordLineType, List<bool>>();
            LinesForUpdatingAtType[CrosswordLineType.Row] = Enumerable.Repeat(true, crossword.Height).ToList();
            LinesForUpdatingAtType[CrosswordLineType.Column] = Enumerable.Repeat(true, crossword.Width).ToList();

        }

        private bool ExistsLinesForUpdating()
        {
            return LinesForUpdatingAtType[CrosswordLineType.Row].Any(i => i) ||
                   LinesForUpdatingAtType[CrosswordLineType.Column].Any(i => i);
        }

        protected abstract bool UpdateLines(CrosswordLineType type);
        
        protected void UpdateLine(CrosswordLineType type, int lineNumber, List<CrosswordCell> cells)
        {
            var invertedLineType = type == CrosswordLineType.Row ? CrosswordLineType.Column : CrosswordLineType.Row;

            var oldCells = GetLineCells(type, lineNumber).Select((cell, column) => Tuple.Create(cell, column)).ToList();
            var newCells = cells.Select((cell, column) => Tuple.Create(cell, column)).ToList();

            newCells.Except(oldCells).ForEach(tuple =>
            {
                LinesForUpdatingAtType[invertedLineType][tuple.Item2] = true;
                if (type == CrosswordLineType.Row)
                    crosswordField[lineNumber, tuple.Item2] = newCells[tuple.Item2].Item1;
                else
                    crosswordField[tuple.Item2, lineNumber] = newCells[tuple.Item2].Item1;
            });
        }

        protected List<CrosswordCell> GetLineCells(CrosswordLineType type, int number)
        {
            return type == CrosswordLineType.Row
                ? crosswordField.GetRowCells(number)
                : crosswordField.GetColumnCells(number);
        }

        protected CrosswordLine GetCrosswordLine(CrosswordLineType type, int number)
        {
            return type == CrosswordLineType.Row ? crossword.Rows[number] : crossword.Columns[number];
        }
    }
}
