using System;
using System.IO;
using JapaneseCrossword.CrosswordUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils.CrosswordTemplateBuilderUtils;

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
            var crossword = new CrosswordSolver(crosswordTemplateBuilder.BuildFromFile(inputFile));
            var solutionStatus = crossword.Solve();
            Console.WriteLine(solutionStatus);
        }
    }
}
