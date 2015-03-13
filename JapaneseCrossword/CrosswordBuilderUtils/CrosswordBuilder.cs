using System;
using System.IO;
using System.Linq;

namespace JapaneseCrossword.CrosswordBuilderUtils
{
    public class CrosswordBuilder : ICrosswordBuilder
    {
        public Crossword BuildFromFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new ArgumentException("No file {0}", file);
            }

            try
            {
                var lines = File.ReadLines(file).ToList();
                var height = int.Parse(lines[0]);

                var rows = lines.Skip(1)
                    .Take(height)
                    .Select(row => new CrosswordLine(row.Split(' ').Where(i => i != "").Select(int.Parse).ToArray()))
                    .ToList();

                var columns = lines.Skip(height + 2)
                    .Select(
                        column => new CrosswordLine(column.Split(' ').Where(i => i != "").Select(int.Parse).ToArray()))
                    .ToList();

                return new Crossword(rows, columns);
            }
            catch (Exception)
            {
                throw new ArgumentException("Incorrect File");
            }
        }
    }
}
