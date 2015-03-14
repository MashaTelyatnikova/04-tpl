using System;
using System.IO;
using JapaneseCrossword.CrosswordBuilderUtils;
using JapaneseCrossword.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordSolverUtils;

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
                var solver = new SingleThreadedCrosswordSolver();
                var crosswordSolution = solver.Solve(crosswordTemplate);

                if (crosswordSolution.Status == CrosswordSolutionStatus.IncorrectCrossword)
                {
                    Console.WriteLine("Incorrect Crossword =(");
                    
                    Environment.Exit(0);
                }

                var crosswordSolutionVisualizer = new CrosswordSolutionVisualizer();
                using (var image = crosswordSolutionVisualizer.Visualize(crosswordSolution))
                {
                    image.Save(outputFile);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
