using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordSolutionUtils;

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
                if (!TryUpdateLines())
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

        protected abstract bool TryUpdateLines();

        protected void UpdateLine(CrosswordLineType type, int lineNumber, List<CrosswordCell> updatedCells)
        {
            var oldCells = GetLineCells(type, lineNumber);
            for (var i = 0; i < oldCells.Count; ++i)
            {
                if (oldCells[i] == updatedCells[i]) continue;
                if (type == CrosswordLineType.Row)
                {
                    LinesForUpdatingAtType[CrosswordLineType.Column][i] = true;
                    crosswordField[lineNumber, i] = updatedCells[i];
                }
                else
                {
                    LinesForUpdatingAtType[CrosswordLineType.Row][i] = true;
                    crosswordField[i, lineNumber] = updatedCells[i];
                }
            }
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
