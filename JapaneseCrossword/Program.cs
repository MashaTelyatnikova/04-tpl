using System;
using System.IO;

namespace JapaneseCrossword
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: \t JapaneseCrossword.exe inputFileName outputFileName");
                
                Environment.Exit(0);
            }
            
            var inputFileName = args[0];
            var outputFileName = args[1];

            if (!File.Exists(inputFileName))
            {
                Console.WriteLine("No input file.");

                Environment.Exit(0);
            }
        }
    }
}
