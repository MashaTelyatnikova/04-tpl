using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils.Enums;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils.Interfaces;
using JapaneseCrossword.CrosswordUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils
{
    public abstract class CrosswordSolver : ICrosswordSolver
    {
        protected Crossword Crossword;
        private List<List<CrosswordSolutionCell>> crosswordCells;
        protected Dictionary<CrosswordLineType, List<bool>> LinesForUpdating;

        public CrosswordSolution Solve(Crossword crosswordParam)
        {
            Crossword = crosswordParam;
            LinesForUpdating = new Dictionary<CrosswordLineType, List<bool>>();

            crosswordCells = Enumerable.Range(0, Crossword.Height)
                                        .Select(i => Enumerable.Range(0, Crossword.Width)
                                                                .Select(j => CrosswordSolutionCell.Unclear).ToList()
                                        ).ToList();

            LinesForUpdating[CrosswordLineType.Row] = Enumerable.Range(0, Crossword.Height)
                                    .Select(i => true)
                                    .ToList();
            LinesForUpdating[CrosswordLineType.Column] = Enumerable.Range(0, Crossword.Width)
                                    .Select(i => true)
                                    .ToList();

            try
            {
                UpdateCrosswordCells();
            }
            catch (ArgumentException)
            {
                return new CrosswordSolution(Enumerable.Empty<List<CrosswordSolutionCell>>().ToList(),
                    SolutionStatus.IncorrectCrossword);
            }

            return new CrosswordSolution(crosswordCells, CrosswordFilled() ? SolutionStatus.Solved : SolutionStatus.PartiallySolved);
        }

        public virtual string Name
        {
            get { throw new NotImplementedException(); }
        }

        private void UpdateCrosswordCells()
        {
            var linesUpdated = true;
            while (linesUpdated)
            {
                var rowsUpdated = UpdateLines(CrosswordLineType.Row);
                var columnsUpdated = UpdateLines(CrosswordLineType.Column);

                linesUpdated = rowsUpdated || columnsUpdated;
            }
        }

        protected abstract bool UpdateLines(CrosswordLineType type);

        private bool CrosswordFilled()
        {
            return !crosswordCells.Any(line => line.Contains(CrosswordSolutionCell.Unclear));
        }

        protected void UpdateLine(CrosswordLine line)
        {
            var blocks = line.Blocks;
            var currentLineCells = GetLineCells(line);

            var lineCellsUpdater = new CrosswordLineCellsUpdater(currentLineCells, blocks);
            var updatedLineCells = lineCellsUpdater.UpdateCells();
            for (var i = 0; i < updatedLineCells.Count; ++i)
            {
                if (currentLineCells[i] != updatedLineCells[i])
                {
                    var invertedType = line.Type == CrosswordLineType.Row
                        ? CrosswordLineType.Column
                        : CrosswordLineType.Row;

                    lock (LinesForUpdating[invertedType])
                    {
                        LinesForUpdating[invertedType][i] = true;
                    }
                }

                lock (crosswordCells)
                {
                    if (line.Type == CrosswordLineType.Row)
                    {
                        crosswordCells[line.Number][i] = updatedLineCells[i];
                    }
                    else
                    {
                        crosswordCells[i][line.Number] = updatedLineCells[i];
                    }
                }
            }
        }

        private List<CrosswordSolutionCell> GetLineCells(CrosswordLine line)
        {
            List<CrosswordSolutionCell> cells;
            if (line.Type == CrosswordLineType.Row)
            {
                cells = crosswordCells[line.Number];
            }
            else
            {
                cells = new List<CrosswordSolutionCell>();
                for (var j = 0; j < Crossword.Height; ++j)
                {
                    cells.Add(crosswordCells[j][line.Number]);
                }
            }

            return cells;
        }
    }
}
