using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var crossword = new CrosswordBuilder().BuildFromFile(inputFile);
                var multiThreadedCrosswordSolver = new SingleThreadedCrosswordSolver();
                var crosswordSolution = multiThreadedCrosswordSolver.Solve(crossword);

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
