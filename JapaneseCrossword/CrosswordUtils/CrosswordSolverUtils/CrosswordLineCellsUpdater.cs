using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using MoreLinq;

namespace JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils
{
    public class CrosswordLineCellsUpdater
    {
        private readonly List<CrosswordSolutionCell> lineCells;
        private int[] blocks;
        private List<bool> maybeFilledCells;
        private List<bool> maybeEmptyCells;

        public CrosswordLineCellsUpdater(IEnumerable<CrosswordSolutionCell> lineCells, int[] blocks)
        {
            this.lineCells = new List<CrosswordSolutionCell>(lineCells);
            this.blocks = blocks;
        }

        public List<CrosswordSolutionCell> UpdateCells()
        {
            maybeFilledCells = MoreEnumerable.Generate(false, k => k).Take(lineCells.Count).ToList();
            maybeEmptyCells = MoreEnumerable.Generate(false, k => k).Take(lineCells.Count).ToList();

            var startBlock = 0;
            var startPosition = -1;
            var dummyBlock = 1;
            blocks = dummyBlock.Concat(blocks).ToArray();

            if (TrySetBlock(startPosition, startBlock))
            {
                for (var k = 0; k < lineCells.Count; ++k)
                {
                    if (lineCells[k] == CrosswordSolutionCell.Unclear && maybeFilledCells[k] ^ maybeEmptyCells[k])
                    {
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
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("Incorrect Crossword");
            }

            return lineCells;
        }

        private bool TrySetBlock(int start, int blockNumber)
        {

            for (var i = start; i >= 0 && i < blocks[blockNumber] + start; ++i)
            {
                if (lineCells[i] == CrosswordSolutionCell.Empty)
                {
                    return false;
                }

            }

            if (blockNumber == blocks.Length - 1)
            {
                for (var i = start + blocks[blockNumber]; i < lineCells.Count; ++i)
                {
                    if (lineCells[i] == CrosswordSolutionCell.Filled)
                    {
                        return false;
                    }
                }

                for (var i = start; i < lineCells.Count; ++i)
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
            for (; nextPosition < lineCells.Count - blocks[blockNumber + 1] + 1; ++nextPosition)
            {
                if (nextPosition > 0 && lineCells[nextPosition - 1] == CrosswordSolutionCell.Filled)
                    break;
                if (TrySetBlock(nextPosition, blockNumber + 1))
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
