﻿using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils.Enums;
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
                    lineCells.Where(cell => cell == CrosswordSolutionCell.Unclear)
                                .ForEach(cell => cell = CrosswordSolutionCell.Empty);
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
            if (start >= 0 && lineCells.Slice(start, blocks[blockNumber]).Contains(CrosswordSolutionCell.Empty))
                return false;

            if (blockNumber == blocks.Length - 1)
            {
                if (lineCells.Slice(start + blocks[blockNumber], lineCells.Count).Contains(CrosswordSolutionCell.Filled))
                    return false;

                for (var i = start; i >= 0 && i < lineCells.Count; ++i)
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
                {
                    break;
                }

                if (!TrySetBlock(nextPosition, blockNumber + 1))
                {
                    continue;
                }

                for (var i = start >= 0 ? start : 0; i < nextPosition; ++i)
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
            return blockIsSet;
        }

        private static bool LineCompleted(IEnumerable<CrosswordSolutionCell> lineCells, IEnumerable<int> blocks)
        {
            return lineCells.Count(c => c == CrosswordSolutionCell.Filled) == blocks.Sum();
        }
    }
}
