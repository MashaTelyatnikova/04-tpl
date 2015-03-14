using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordSolverUtils;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordFieldTests
    {
        [Test]
        public void EmptyField()
        {
            var crosswordField = new CrosswordField(0, 0);

            Assert.That(crosswordField.IsFilled(), Is.True);
            Assert.That(crosswordField.ToMatrix(), Is.EqualTo(Enumerable.Empty<List<CrosswordCell>>()));
        }

        [Test]
        public void GetRowCells_ForComplexField_ReturnCorrectResult()
        {
            var crosswordField = new CrosswordField(2, 3);

            crosswordField[0, 0] = CrosswordCell.Filled;
            crosswordField[0, 1] = CrosswordCell.Empty;
            crosswordField[1, 0] = CrosswordCell.Filled;

            Assert.That(crosswordField.GetRowCells(0), Is.EqualTo(new[] { CrosswordCell.Filled, CrosswordCell.Empty }));
            Assert.That(crosswordField.GetRowCells(1), Is.EqualTo(new[] { CrosswordCell.Filled, CrosswordCell.Unclear }));
            Assert.That(crosswordField.GetRowCells(2), Is.EqualTo(new[] { CrosswordCell.Unclear, CrosswordCell.Unclear }));
        }

        [Test]
        public void GetColumnCells_ForComplexField_ReturnCorrectResult()
        {
            var crosswordField = new CrosswordField(3, 3);

            crosswordField[0, 0] = CrosswordCell.Filled;
            crosswordField[0, 1] = CrosswordCell.Empty;
            crosswordField[1, 0] = CrosswordCell.Filled;
            crosswordField[2, 2] = CrosswordCell.Filled;

            Assert.That(crosswordField.GetColumnCells(0), Is.EqualTo(new[] { CrosswordCell.Filled, CrosswordCell.Filled, CrosswordCell.Unclear }));
            Assert.That(crosswordField.GetColumnCells(1), Is.EqualTo(new[] { CrosswordCell.Empty, CrosswordCell.Unclear, CrosswordCell.Unclear }));
            Assert.That(crosswordField.GetColumnCells(2), Is.EqualTo(new[] { CrosswordCell.Unclear, CrosswordCell.Unclear, CrosswordCell.Filled }));
        }

        [Test]
        public void IsFilled_ForComplexField_ReturnCorrectResult()
        {
            var crosswordField = new CrosswordField(2, 2);

            crosswordField[0, 0] = CrosswordCell.Filled;
            crosswordField[0, 1] = CrosswordCell.Empty;
            crosswordField[1, 0] = CrosswordCell.Filled;

            Assert.That(crosswordField.IsFilled(), Is.False);

            crosswordField[1, 1] = CrosswordCell.Filled;
            Assert.That(crosswordField.IsFilled(), Is.True);
        }

        [Test]
        public void ToMatrix_ForComplexField_ReturnCorrectResult()
        {
            var crosswordField = new CrosswordField(2, 2);

            crosswordField[0, 0] = CrosswordCell.Filled;
            crosswordField[0, 1] = CrosswordCell.Empty;
            crosswordField[1, 0] = CrosswordCell.Filled;

            Assert.That(crosswordField.ToMatrix(), Is.EqualTo(new[] { new[] { CrosswordCell.Filled, CrosswordCell.Empty, }, new[] { CrosswordCell.Filled, CrosswordCell.Unclear } }));
        }
    }
}
