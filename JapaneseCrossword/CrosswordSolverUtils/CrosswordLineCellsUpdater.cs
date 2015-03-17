using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace JapaneseCrossword.CrosswordSolverUtils
{
    public class CrosswordLineCellsUpdater
    {
        private readonly List<CrosswordCell> lineCells;
        private readonly CrosswordLine line;
        private readonly BlockPositionState[,] blockPositionStates;
        private readonly List<bool> maybeFilledCells;
        private readonly List<bool> maybeEmptyCells;

        public CrosswordLineCellsUpdater(IReadOnlyCollection<CrosswordCell> lineCells, CrosswordLine line)
        {
            this.lineCells = new List<CrosswordCell>(lineCells);
            this.line = line;

            blockPositionStates = new BlockPositionState[lineCells.Count + 1, line.BlocksCount + 1];
            maybeFilledCells = Enumerable.Repeat(false, lineCells.Count).ToList();
            maybeEmptyCells = Enumerable.Repeat(false, lineCells.Count).ToList();
        }

        public List<CrosswordCell> GetUpdatedCells()
        {
            if (TrySetBlock(0, 0))
            {
                UpdateCells();
            }
            else
            {
                return null;
            }

            return lineCells;
        }

        private bool TrySetBlock(int start, int blockNumber)
        {
            if (blockPositionStates[start, blockNumber] != BlockPositionState.StillUnknown)
                return blockPositionStates[start, blockNumber] == BlockPositionState.Successfully;

            if (BlockContainsEmptyCells(start, blockNumber))
            {
                blockPositionStates[start, blockNumber] = BlockPositionState.Fails;
                return false;
            }

            if (blockNumber < line.BlocksCount - 1)
            {
                return TrySetMiddleBlock(start, blockNumber);
            }

            return TrySetLastBlock(start, blockNumber);
        }

        private bool BlockContainsEmptyCells(int start, int blockNumber)
        {
            return lineCells.Slice(start, line.GetBlockLength(blockNumber)).Contains(CrosswordCell.Empty);
        }

        private bool TrySetMiddleBlock(int start, int blockNumber)
        {
            var blockLength = line.GetBlockLength(blockNumber);
            var nextPosition = blockLength == 0 ? start : start + blockLength + 1;
            var nextBlockLength = line.GetBlockLength(blockNumber + 1);
            var lastPosition = lineCells.Count - nextBlockLength;
            var blockIsSet = false;

            while (nextPosition <= lastPosition && !ExistsFilledCellBeforeBlockPosition(nextPosition))
            {
                if (TrySetBlock(nextPosition, blockNumber + 1))
                {
                    SetBlock(start, nextPosition, blockNumber);
                    blockIsSet = true;
                }
                nextPosition++;
            }

            return blockIsSet;
        }

        private bool ExistsFilledCellBeforeBlockPosition(int blockPosition)
        {
            return blockPosition > 0 && lineCells[blockPosition - 1] == CrosswordCell.Filled;
        }

        private bool TrySetLastBlock(int start, int blockNumber)
        {
            if (AfterBlockExistsFilledCells(start, blockNumber))
                return false;

            SetBlock(start, lineCells.Count, blockNumber);
            return true;
        }

        private bool AfterBlockExistsFilledCells(int start, int blockNumber)
        {
            var blockLength = line.GetBlockLength(blockNumber);
            return blockLength > 0 && lineCells.Slice(start + blockLength, lineCells.Count).Contains(CrosswordCell.Filled);
        }

        private void SetBlock(int start, int end, int blockNumber)
        {
            var blockLength = line.GetBlockLength(blockNumber);
            for (var i = start; i < end; ++i)
            {
                if (i < start + blockLength)
                {
                    maybeFilledCells[i] = true;
                }
                else
                {
                    maybeEmptyCells[i] = true;
                }
            }

            blockPositionStates[start, blockNumber] = BlockPositionState.Successfully;
        }

        private void UpdateCells()
        {
            for (var k = 0; k < lineCells.Count; ++k)
            {
                if (lineCells[k] == CrosswordCell.Unclear && maybeFilledCells[k] ^ maybeEmptyCells[k])
                {
                    lineCells[k] = maybeFilledCells[k] ? CrosswordCell.Filled : CrosswordCell.Empty;
                }
            }
        }
    }
}
