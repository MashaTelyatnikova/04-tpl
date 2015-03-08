using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils
{
    public class CrosswordTemplate
    {
        private List<CrosswordTemplateRow> Rows { get; set; }
        private List<CrosswordTemplateColumn> Columns { get; set; }

        public CrosswordTemplate(IEnumerable<CrosswordTemplateRow> rows, IEnumerable<CrosswordTemplateColumn> columns)
        {
            this.Rows = rows as List<CrosswordTemplateRow> ?? rows.ToList();
            this.Columns = columns as List<CrosswordTemplateColumn> ?? columns.ToList();
        }

        public override bool Equals(object obj)
        {
            var other = (CrosswordTemplate)obj;
            return Rows.SequenceEqual(other.Rows) && Columns.SequenceEqual(other.Columns);
        }
    }
}
