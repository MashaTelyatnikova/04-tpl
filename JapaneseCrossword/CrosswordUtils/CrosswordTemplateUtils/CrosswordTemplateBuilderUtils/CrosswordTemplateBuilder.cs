using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JapaneseCrossword.FileReaderUtils;

namespace JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils.CrosswordTemplateBuilderUtils
{
    public class CrosswordTemplateBuilder : ICrosswordTemplateBuilder
    {
        public CrosswordTemplate BuildFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException("No file {0}", fileName);
            }


            var fileReader = new FileReader(fileName);

            return new CrosswordTemplate(BuildRows(fileReader), BuildColumns(fileReader));
        }

        private static IEnumerable<CrosswordTemplateRow> BuildRows(FileReader fileReader)
        {
            var rowsCount = fileReader.ReadNextInt();

            for (var i = 0; i < rowsCount; ++i)
            {
                int? groupsCount = fileReader.ReadNextInt();

                if (groupsCount == null)
                {
                    throw new ArgumentException("Incorrect file");
                }

                var groups = fileReader.ReadNextInts(groupsCount.Value);

                var enumerable = groups as int?[] ?? groups.ToArray();

                if (enumerable.Any(g => g == null))
                {
                    throw new ArgumentException("Incorrect file");
                }


                yield return new CrosswordTemplateRow(enumerable.Select(g => g.Value).ToArray());
            }
        }

        private static IEnumerable<CrosswordTemplateColumn> BuildColumns(FileReader fileReader)
        {
            var columnsCount = fileReader.ReadNextInt();

            for (var i = 0; i < columnsCount; ++i)
            {
                int? groupsCount = fileReader.ReadNextInt();

                if (groupsCount == null)
                {
                    throw new ArgumentException("Incorrect file");
                }

                var groups = fileReader.ReadNextInts(groupsCount.Value);

                var enumerable = groups as int?[] ?? groups.ToArray();

                if (enumerable.Any(g => g == null))
                {
                    throw new ArgumentException("Incorrect file");
                }

                yield return new CrosswordTemplateColumn(enumerable.Select(g => g.Value).ToArray());
            }
        }
    }
}
