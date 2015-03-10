using System.Linq;
using JapaneseCrossword.CrosswordUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils
{
    public class CrosswordLine
    {
        public int Number { get; private set; }
        public CrosswordLineType Type { get; private set; }
        public int[] Blocks { get; private set; }

        public CrosswordLine(int number, CrosswordLineType type,params int [] blocks)
        {
            Number = number;
            Type = type;
            Blocks = blocks;
        }

        public override bool Equals(object obj)
        {
            var other = (CrosswordLine) obj;
            return Number == other.Number && Type == other.Type && Blocks.SequenceEqual(other.Blocks);
        }
    }
}
