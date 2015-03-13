using System.Collections.Generic;

namespace JapaneseCrossword
{
    public class Crossword
    {
        public List<CrosswordLine> Rows { get; private set; }
        public List<CrosswordLine> Columns { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Crossword(List<CrosswordLine> rows, List<CrosswordLine> columns)
        {
            Rows = rows;
            Columns = columns;
            Width = columns.Count;
            Height = rows.Count;
        }
    }
}
