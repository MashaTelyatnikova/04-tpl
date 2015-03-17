using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JapaneseCrossword;
using JapaneseCrossword.CrosswordSolverUtils;

namespace CrosswordSolversComparer
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var singleThreadedCrosswordSolver = new SingleThreadedCrosswordSolver();
            var multiThreadedCrosswordSolver = new MultiThreadedCrosswordSolver();

            var crosswordGenerator = new CrosswordGenerator();
            var singleThreadedResults = new Dictionary<int, List<double>>();
            var multiThreadedResults = new Dictionary<int, List<double>>();

            for (var width = 1; width < 25; ++width)
            {
                for (var height = 1; height < 25; ++height)
                {
                    for (var i = 0; i < 30; ++i)
                    {
                        var crossword = crosswordGenerator.Next(width, width + 1, height, height + 1);
                        var singleThreadedResult = TestSolver(singleThreadedCrosswordSolver, crossword.Item1);
                        var multiThreadedResult = TestSolver(multiThreadedCrosswordSolver, crossword.Item1);
                        if (!singleThreadedResults.ContainsKey(width * height))
                            singleThreadedResults[width * height] = new List<double>();
                        if (!multiThreadedResults.ContainsKey(width * height))
                            multiThreadedResults[width * height] = new List<double>();

                        singleThreadedResults[width * height].Add(singleThreadedResult);
                        multiThreadedResults[width * height].Add(multiThreadedResult);
                    }

                }
            }

            var single = new List<string>();
            var multi = new List<string>();
            var result = 0.0;
            var count = 0;
            foreach (var key in singleThreadedResults.Keys.OrderBy(i => i))
            {
                double s = singleThreadedResults[key].Sum() / singleThreadedResults[key].Count;
                double m = multiThreadedResults[key].Sum() / multiThreadedResults[key].Count;
                single.Add(string.Format("{0}", s));
                multi.Add(string.Format("{0}", m));
                if (s > m)
                {
                    var percent = (m * 100) / s;
                    result += percent;
                    count++;
                }
            }

            Console.WriteLine(result / count);

            File.WriteAllLines("single.csv", single);
            File.WriteAllLines("multi.csv", multi);

        }

        private static double TestSolver(ICrosswordSolver solver, Crossword crossword)
        {
            var stopwatch = new Stopwatch();
            GC.Collect();
            stopwatch.Start();
            solver.Solve(crossword);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
