using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using MathNet.Numerics.Random;
namespace JapaneseCrossword
{
    public class CrosswordGenerator
    {
        private readonly Random random;

        public CrosswordGenerator()
        {
            random = new Random();
        }

        public Tuple<Crossword, string> Next(int minWidth, int maxWidth, int minHeight, int maxHeight, double fillingCellProbability = 0.5)
        {
            var crosswordLines = GenerateCrosswordLines(minWidth, maxWidth, minHeight, maxHeight, fillingCellProbability);
            var crossword = GetCrosswordFromLines(crosswordLines);

            return Tuple.Create(crossword, crosswordLines.ToDelimitedString("\n"));
        }

        private List<string> GenerateCrosswordLines(int minWidth, int maxWidth, int minHeight, int maxHeight, double fillingCellProbability)
        {
            var width = random.Next(minWidth, maxWidth);
            var height = random.Next(minHeight, maxHeight);
            var crosswordLines = new List<string>();
            for (var x = 0; x < height; ++x)
            {
                var row = new StringBuilder();
                for (var y = 0; y < width; ++y)
                {
                    var next = random.NextDouble();
                 
                    row.Append(next < fillingCellProbability ? '*' : '.');
                }

                crosswordLines.Add(row.ToString());
            }

            return crosswordLines;
        }

        private static Crossword GetCrosswordFromLines(List<string> crosswordLines)
        {
            var height = crosswordLines.Count;
            var width = height == 0 ? 0 : crosswordLines[0].Length;

            var rows = crosswordLines.Select(line => line.Split('.').Where(s => s != "").Select(s => s.Length).ToArray())
                                .Select(blocks => new CrosswordLine(blocks))
                                .ToList();

            var columns = Enumerable.Range(0, width)
                                    .Select(y => crosswordLines.Select(row => row[y]).ToDelimitedString(""))
                                    .Select(line => line.Split('.').Where(s => s != "").Select(s => s.Length).ToArray())
                                    .Select(blocks => new CrosswordLine(blocks))
                                    .ToList();
            return new Crossword(rows, columns);
        }
    }
}