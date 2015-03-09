using System;
using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils
{
    public class CrosswordTemplate
    {
        private readonly List<CrosswordTemplateRow> rows;
        private readonly List<CrosswordTemplateColumn> columns;
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public CrosswordTemplate(IEnumerable<CrosswordTemplateRow> rows, IEnumerable<CrosswordTemplateColumn> columns)
        {
            this.rows = rows as List<CrosswordTemplateRow> ?? rows.ToList();
            this.columns = columns as List<CrosswordTemplateColumn> ?? columns.ToList();
            this.Width = this.columns.Count;
            this.Height = this.rows.Count;
        }

        public IEnumerable<int> GetBlocksFromRow(int number)
        {
            if (number >= 0 && number < rows.Count)
                return rows[number].groupsFilledCells;

            throw new ArgumentOutOfRangeException();
        }

        public IEnumerable<int> GetBlocksFromColumn(int number)
        {
            if (number >= 0 && number < columns.Count)
                return columns[number].blocksFilledCells;

            throw new ArgumentOutOfRangeException();
        } 

        public override bool Equals(object obj)
        {
            var other = (CrosswordTemplate)obj;
            return rows.SequenceEqual(other.rows) && columns.SequenceEqual(other.columns);
        }
    }
}
