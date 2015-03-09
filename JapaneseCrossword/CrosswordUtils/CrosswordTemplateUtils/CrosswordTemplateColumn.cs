using System.Collections.Generic;
using System.Linq;

namespace JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils
{
    public class CrosswordTemplateColumn
    {
        public readonly List<int> blocksFilledCells;
        public CrosswordTemplateColumn(params int[] groupsFilledCells)
        {
            this.blocksFilledCells = groupsFilledCells.ToList();
        }

        public override bool Equals(object obj)
        {
            var other = (CrosswordTemplateColumn)obj;
            return blocksFilledCells.SequenceEqual(other.blocksFilledCells);
        }
    }
}
