using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils;
using MoreLinq;

namespace JapaneseCrossword.CrosswordUtils
{
    public class CrosswordSolver : ICrosswordSolver
    {
        private readonly CrosswordTemplate crosswordTemplate;
        private List<List<CrosswordSolutionCell>> crosswordCells;
        private List<bool> rowsForUpdating;
        private List<bool> columnsForUpdating;

        public CrosswordSolver(CrosswordTemplate crosswordTemplate)
        {
            this.crosswordTemplate = crosswordTemplate;
        }

        public CrosswordSolution Solve()
        {
            crosswordCells = Enumerable.Range(0, crosswordTemplate.Height)
                                        .Select(i => Enumerable.Range(0, crosswordTemplate.Width)
                                                                .Select(j => CrosswordSolutionCell.Unclear).ToList()
                                        ).ToList();

            rowsForUpdating = Enumerable.Range(0, crosswordTemplate.Height)
                                    .Select(i => true)
                                    .ToList();
            columnsForUpdating = Enumerable.Range(0, crosswordTemplate.Width)
                                    .Select(i => true)
                                    .ToList();


            try
            {
                UpdateCrosswordCells();
            }
            catch (ArgumentException exception)
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
                var rowsUpdated = UpdateLines(rowsForUpdating, LineType.Row);
                var columnsUpdated = UpdateLines(columnsForUpdating, LineType.Column);
                var solution = new CrosswordSolution(crosswordCells, SolutionStatus.PartiallySolved).ToString();
                linesUpdated = rowsUpdated || columnsUpdated;
            }
        }

        private bool UpdateLines(List<bool> linesForUpdating, LineType type)
        {
            var linesUpdated = false;
            for (var lineNumber = 0; lineNumber < linesForUpdating.Count; ++lineNumber)
            {
                if (linesForUpdating[lineNumber])
                {
                    linesUpdated = true;
                    linesForUpdating[lineNumber] = false;
                    UpdateLine(lineNumber, type);
                    var solution = new CrosswordSolution(crosswordCells, SolutionStatus.PartiallySolved).ToString();
                    var d = 0;
                }
            }

            return linesUpdated;
        }

        private bool CrosswordFilled()
        {
            return !crosswordCells.Any(line => line.Contains(CrosswordSolutionCell.Unclear));
        }

        private void UpdateLine(int lineNumber, LineType type)
        {
            var blocks = new List<int>() { 1 };
            var lineCells = new List<CrosswordSolutionCell>();

            if (type == LineType.Row)
            {
                blocks.AddRange(crosswordTemplate.GetBlocksFromRow(lineNumber));
                lineCells = crosswordCells[lineNumber];
            }
            else
            {
                blocks.AddRange(crosswordTemplate.GetBlocksFromColumn(lineNumber));
                for (var j = 0; j < crosswordTemplate.Height; ++j)
                {
                    lineCells.Add(crosswordCells[j][lineNumber]);
                }
            }

            var maybeFilledCells = MoreEnumerable.Generate(false, k => k).Take(lineCells.Count).ToList();
            var maybeEmptyCells = MoreEnumerable.Generate(false, k => k).Take(lineCells.Count).ToList();

            var startBlock = 0;
            var startPosition = -1;

            if (TrySetBlock(lineCells, startPosition, startBlock, blocks, maybeFilledCells, maybeEmptyCells))
            {
              
                for (var k = 0; k < lineCells.Count; ++k)
                {
                    if (lineCells[k] == CrosswordSolutionCell.Unclear && maybeFilledCells[k] ^ maybeEmptyCells[k])
                    {
                        if (type == LineType.Row)
                            columnsForUpdating[k] = true;
                        else
                        {
                            rowsForUpdating[k] = true;
                        }
                        lineCells[k] = maybeFilledCells[k] ? CrosswordSolutionCell.Filled : CrosswordSolutionCell.Empty;
                    }
                }
                if (LineCompleted(lineCells, blocks.Skip(1).ToList()))
                {
                    for (var j = 0; j < lineCells.Count; ++j)
                    {
                        if (lineCells[j] == CrosswordSolutionCell.Unclear)
                        {
                            lineCells[j] = CrosswordSolutionCell.Empty;
                            if (type == LineType.Row)
                                columnsForUpdating[j] = true;
                            else
                            {
                                rowsForUpdating[j] = true;
                            }
                        }

                    }
                }
                for (var i = 0; i < lineCells.Count; ++i)
                {
                    if (type == LineType.Row)
                        crosswordCells[lineNumber][i] = lineCells[i];
                    else
                    {
                        crosswordCells[i][lineNumber] = lineCells[i];
                    }
                }
            }
            else
            {
                throw new ArgumentException("Incorrect crossword.");
            }
        }

        private bool TrySetBlock(List<CrosswordSolutionCell> cells, int start, int blockNumber, List<int> blocks, List<bool> maybeFilledCells, List<bool> maybeEmptyCells)
        {

            for (var i = start; i >= 0 && i < blocks[blockNumber] + start; ++i)
            {
                if (cells[i] == CrosswordSolutionCell.Empty)
                {
                    return false;
                }
                    
            }

            if (blockNumber == blocks.Count - 1)
            {
                for (var i = start + blocks[blockNumber]; i < cells.Count; ++i)
                {
                    if (cells[i] == CrosswordSolutionCell.Filled)
                    {
                        return false;
                    }
                }

                for (var i = start; i < cells.Count; ++i)
                {
                    if (i >= start && i < start + blocks[blockNumber])
                    {
                        maybeFilledCells[i] = true;
                    }
                    else
                    {
                        maybeEmptyCells[i] = true;
                    }
                }

                return true;
            }
            var nextPosition = start + blocks[blockNumber] + (start == -1 ? 0 : 1);
            var blockIsSet = false;
            for (;nextPosition < cells.Count - blocks[blockNumber + 1] + 1; ++nextPosition)
            {
                if (nextPosition > 0 && cells[nextPosition - 1] == CrosswordSolutionCell.Filled)
                    break;
                if (TrySetBlock(cells, nextPosition, blockNumber + 1, blocks, maybeFilledCells, maybeEmptyCells))
                {
                    var i = start >= 0 ? start : 0;

                    for (; i < nextPosition; ++i)
                    {
                        if (i >= start && i < start + blocks[blockNumber])
                        {
                            maybeFilledCells[i] = true;
                        }
                        else
                        {
                            maybeEmptyCells[i] = true;
                        }
                    }

                    blockIsSet = true;
                }
            }
            return blockIsSet;
        }

        private static bool LineCompleted(IEnumerable<CrosswordSolutionCell> lineCells, IEnumerable<int> blocks)
        {
            return lineCells.Count(c => c == CrosswordSolutionCell.Filled) == blocks.Sum();
        }
    }
}
