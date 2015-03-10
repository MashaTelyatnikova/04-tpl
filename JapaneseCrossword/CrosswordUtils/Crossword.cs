using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.Enums;

namespace JapaneseCrossword.CrosswordUtils
{
    public class Crossword
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly IEnumerable<CrosswordLine> lines;
        public Crossword(IEnumerable<CrosswordLine> lines)
        {
            lines = lines as List<CrosswordLine> ?? lines.ToList();
            if (lines == null)
                throw new ArgumentNullException();

            Width = lines.Count(line => line.Type == CrosswordLineType.Column);
            Height = lines.Count(line => line.Type == CrosswordLineType.Row);
            this.lines = lines;
        }

        public CrosswordLine GetLine(int lineNumber, CrosswordLineType type)
        {
            return lines.FirstOrDefault(x => x.Number == lineNumber && x.Type == type);
        }

        public override bool Equals(object obj)
        {
            var other = (Crossword)obj;
            return Width == other.Width && Height == other.Height && lines.SequenceEqual(other.lines);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (lines != null ? lines.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Width;
                hashCode = (hashCode * 397) ^ Height;
                return hashCode;
            }
        }
    }
}
