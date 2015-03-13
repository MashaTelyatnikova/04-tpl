using System.Linq;
using MoreLinq;

namespace JapaneseCrossword
{
    public class CrosswordLine
    {
        private readonly int[] blocks;

        public int BlocksCount
        {
            get { return blocks.Count(); }
        }

        public CrosswordLine(params int[] blocks)
        {
            this.blocks = 0.Concat(blocks).ToArray();
        }

        public int GetBlockLength(int blockNumber)
        {
            return blockNumber >= 0 && blockNumber < blocks.Length ? blocks[blockNumber] : 0;
        }

        public override string ToString()
        {
            return string.Format("[{0}]", blocks.ToDelimitedString(" , "));
        }
    }
}
