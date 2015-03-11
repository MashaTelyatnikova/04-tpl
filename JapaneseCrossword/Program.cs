using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilderUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils.Interfaces;

namespace JapaneseCrossword
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: \t JapaneseCrossword.exe inputFileName outputFileName");

                Environment.Exit(0);
            }

            var inputFile = args[0];
            var outputFile = args[1];

            if (!File.Exists(inputFile))
            {
                Console.WriteLine("No input file {0}.", inputFile);

                Environment.Exit(0);
            }

            try
            {
                var crosswordTemplate = new CrosswordBuilder().BuildFromFile(inputFile);
                var solver = new MultiThreadedCrosswordSolver();
                var crosswordSolution = solver.Solve(crosswordTemplate);

                var crosswordSolutionVisualizer = new CrosswordSolutionVisualizer(outputFile);
                crosswordSolutionVisualizer.Visualize(crosswordSolution);

                var chart = CrosswordSolversComparer.Compare(
                    new ICrosswordSolver[] { new MultiThreadedCrosswordSolver(), new SingleThreadedCrosswordSolver() },
                    new[] { Color.Blue, Color.Red });
                
                chart.SaveImage("result.png", ChartImageFormat.Png);
                chart.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
