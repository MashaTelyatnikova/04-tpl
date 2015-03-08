using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils
{
    public class CrosswordTemplateColumn
    {
        private readonly List<int> groupsFilledCells;
        public CrosswordTemplateColumn(params int[] groupsFilledCells)
        {
            this.groupsFilledCells = groupsFilledCells.ToList();
        }

        public override bool Equals(object obj)
        {
            var other = (CrosswordTemplateColumn)obj;
            return groupsFilledCells.SequenceEqual(other.groupsFilledCells);
        }
    }
}
