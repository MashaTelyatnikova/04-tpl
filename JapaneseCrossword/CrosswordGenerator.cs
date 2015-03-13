using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;

namespace JapaneseCrossword
{
    public class CrosswordGenerator
    {
        private readonly Random random;

        public CrosswordGenerator()
        {
            random = new Random();
        }
        
        public Tuple<Crossword, string> Next(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var width = random.Next(minWidth, maxWidth);
            var height = random.Next(minHeight, maxHeight);
            var crossword = new List<string>();
            for (var x = 0; x < height; ++x)
            {
                var row = new StringBuilder();
                for (var y = 0; y < width; ++y)
                {
                    row.Append(random.Next(0, 2) == 0 ? '.' : '*');
                }

                crossword.Add(row.ToString());
            }

            var rows= new List<CrosswordLine>();
            var columns = new List<CrosswordLine>();
            for (var x = 0; x < height; ++x)
            {
                var blocks = crossword[x].Split('.').Where(s => s != "").Select(s => s.Length).ToArray();
                rows.Add(new CrosswordLine( blocks));
            }

            for (var y = 0; y < width; ++y)
            {
                var column = new StringBuilder();
                for (var x = 0; x < height; ++x)
                {
                    column.Append(crossword[x][y]);
                }
                var blocks = column.ToString().Split('.').Where(s => s != "").Select(s => s.Length).ToArray();
                columns.Add(new CrosswordLine(blocks));
            }

            return Tuple.Create(new Crossword(rows, columns), crossword.ToDelimitedString("\n"));
        }
    }
}
