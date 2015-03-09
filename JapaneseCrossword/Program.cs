using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JapaneseCrossword.CrosswordUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils.CrosswordTemplateBuilderUtils;
using MoreLinq;

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

            var crosswordTemplateBuilder = new CrosswordTemplateBuilder();

            try
            {
                var crossword = new CrosswordSolver(crosswordTemplateBuilder.BuildFromFile(inputFile));
                var crosswordSolution = crossword.Solve();

                var crosswordSolutionVisualizer = new CrosswordSolutionVisualizer(outputFile);
                crosswordSolutionVisualizer.Visualize(crosswordSolution);

            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
