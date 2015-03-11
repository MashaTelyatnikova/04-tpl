using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilderUtils.Interfaces;
using JapaneseCrossword.CrosswordUtils.Enums;
using JapaneseCrossword.FileReaderUtils;

namespace JapaneseCrossword.CrosswordUtils.CrosswordBuilderUtils
{
    public class CrosswordBuilder : ICrosswordBuilder
    {
        public Crossword BuildFromFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new ArgumentException("No file {0}", file);
            }

            var lines = new List<CrosswordLine>();

            var fileReader = new FileReader(file);
            var height = fileReader.ReadNextInt();
            lines.AddRange(GetNextLines(height.GetValueOrDefault(), CrosswordLineType.Row, fileReader));

            var width = fileReader.ReadNextInt();
            lines.AddRange(GetNextLines(width.GetValueOrDefault(), CrosswordLineType.Column, fileReader));
            return new Crossword(lines);

        }

        private IEnumerable<CrosswordLine> GetNextLines(int count, CrosswordLineType type, FileReader fileReader)
        {
            for (var i = 0; i < count; ++i)
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

                yield return new CrosswordLine(i, type, enumerable.Select(e => e.GetValueOrDefault()).ToArray());
            }
        }
    }
}
