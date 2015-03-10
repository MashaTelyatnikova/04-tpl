using System.Linq;
using JapaneseCrossword.CrosswordUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils
{
    public class CrosswordLine
    {
        public int Number { get; private set; }
        public CrosswordLineType Type { get; private set; }
        public int[] Blocks { get; private set; }

        public CrosswordLine(int number, CrosswordLineType type, params int[] blocks)
        {
            Number = number;
            Type = type;
            Blocks = blocks;
        }

        public override bool Equals(object obj)
        {
            var other = (CrosswordLine)obj;
            return Number == other.Number && Type == other.Type && Blocks.SequenceEqual(other.Blocks);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Number;
                hashCode = (hashCode * 397) ^ (int)Type;
                hashCode = (hashCode * 397) ^ (Blocks != null ? Blocks.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
