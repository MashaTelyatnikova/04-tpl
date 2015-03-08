using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils
{
    public class CrosswordTemplateRow
    {
        private readonly List<int> groupsFilledCells;
        public CrosswordTemplateRow(params int[] groupsFilledCells)
        {
            this.groupsFilledCells = groupsFilledCells.ToList();
        }

        public override bool Equals(object obj)
        {
            var other = (CrosswordTemplateRow)obj;

            return groupsFilledCells.SequenceEqual(other.groupsFilledCells);
        }
    }
}
