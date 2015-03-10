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
        private List<bool> rowsForUpdating;
        private List<bool> columnsForUpdating;

        public CrosswordSolution Solve(Crossword crosswordParam)
        {
            this.Crossword = crosswordParam;
            crosswordCells = Enumerable.Range(0, Crossword.Height)
                                        .Select(i => Enumerable.Range(0, Crossword.Width)
                                                                .Select(j => CrosswordSolutionCell.Unclear).ToList()
                                        ).ToList();

            rowsForUpdating = Enumerable.Range(0, Crossword.Height)
                                    .Select(i => true)
                                    .ToList();
            columnsForUpdating = Enumerable.Range(0, Crossword.Width)
                                    .Select(i => true)
                                    .ToList();

            try
            {
                UpdateCrosswordCells();
            }
            catch (ArgumentException)
            {
                return new CrosswordSolution(Enumerable.Empty<List<CrosswordSolutionCell>>().ToList(), SolutionStatus.IncorrectCrossword);
            }

            return new CrosswordSolution(crosswordCells, CrosswordFilled() ? SolutionStatus.Solved : SolutionStatus.PartiallySolved);
        }

        private void UpdateCrosswordCells()
        {
            var linesUpdated = true;
            while (linesUpdated)
            {
                var rowsUpdated = UpdateLines(rowsForUpdating, CrosswordLineType.Row);
                var columnsUpdated = UpdateLines(columnsForUpdating, CrosswordLineType.Column);
                linesUpdated = rowsUpdated || columnsUpdated;
            }
        }

        protected abstract bool UpdateLines(List<bool> linesForUpdating, CrosswordLineType type);

        private bool CrosswordFilled()
        {
            return !crosswordCells.Any(line => line.Contains(CrosswordSolutionCell.Unclear));
        }

        protected void UpdateLine(CrosswordLine line)
        {
            var blocks = line.Blocks;
            var currentLineCells = new List<CrosswordSolutionCell>();

            if (line.Type == CrosswordLineType.Row)
            {
                currentLineCells = crosswordCells[line.Number];
            }
            else
            {
                for (var j = 0; j < Crossword.Height; ++j)
                {
                    currentLineCells.Add(crosswordCells[j][line.Number]);
                }
            }

            var lineCellsUpdater = new CrosswordLineCellsUpdater(currentLineCells, blocks);
            var updatedLineCells = lineCellsUpdater.UpdateCells();
            for (var i = 0; i < updatedLineCells.Count; ++i)
            {
                if (line.Type == CrosswordLineType.Row)
                {
                    if (currentLineCells[i] != updatedLineCells[i])
                        lock (columnsForUpdating)
                            columnsForUpdating[i] = true;

                    lock (crosswordCells)
                        crosswordCells[line.Number][i] = updatedLineCells[i];
                }
                else
                {
                    if (currentLineCells[i] != updatedLineCells[i])
                        lock (rowsForUpdating)
                            rowsForUpdating[i] = true;

                    lock (crosswordCells)
                        crosswordCells[i][line.Number] = updatedLineCells[i];
                }
            }
        }
    }
}
