using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilder.Interfaces;
using JapaneseCrossword.CrosswordUtils.Enums;
using JapaneseCrossword.FileReaderUtils;

namespace JapaneseCrossword.CrosswordUtils.CrosswordBuilder
{
    public class CrosswordBuilder : ICrosswordBuilder
    {
        public Crossword BuildFromFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new ArgumentException("No file {0}", file);
            }


            var fileReader = new FileReader(file);
            var height = fileReader.ReadNextInt();

            var lines = new List<CrosswordLine>();
            for (var i = 0; i < height; ++i)
            {
                int? blocksCount = fileReader.ReadNextInt();

                if (blocksCount == null)
                {
                    throw new ArgumentException("Incorrect file");
                }

                var blocks = fileReader.ReadNextInts(blocksCount.Value);

                var enumerable = blocks as List<int?> ?? blocks.ToList();

                if (enumerable.Any(g => g == null))
                {
                    throw new ArgumentException("Incorrect file");
                }

                lines.Add(new CrosswordLine(i, CrosswordLineType.Row, enumerable.Select(e => e.Value).ToArray()));
            }

            var width = fileReader.ReadNextInt();
            for (var i = 0; i < width; ++i)
            {
                int? blocksCount = fileReader.ReadNextInt();

                if (blocksCount == null)
                {
                    throw new ArgumentException("Incorrect file");
                }

                var blocks = fileReader.ReadNextInts(blocksCount.Value);

                var enumerable = blocks as List<int?> ?? blocks.ToList();

                if (enumerable.Any(g => g == null))
                {
                    throw new ArgumentException("Incorrect file");
                }

                lines.Add(new CrosswordLine(i, CrosswordLineType.Column, enumerable.Select(e => e.Value).ToArray()));
            }

            return new Crossword(width.GetValueOrDefault(), height.GetValueOrDefault(), lines);

        }
    }
}
