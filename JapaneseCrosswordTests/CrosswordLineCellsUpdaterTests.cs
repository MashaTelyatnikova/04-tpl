using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword;
using JapaneseCrossword.CrosswordSolverUtils;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordLineCellsUpdaterTests
    {
        [TestCaseSource("GetGoodInputs")]
        public void UpdateCells_ForCorrectLine_ReturnCorrectResult(string initialCells, CrosswordLine line,
            string expectedCells)
        {
            var solver = new CrosswordLineCellsUpdater(GetCellsFromString(initialCells), line);
            var solution = solver.GetUpdatedCells();

            Assert.That(solution, Is.EqualTo(GetCellsFromString(expectedCells)));
        }


        [TestCaseSource("GetBadInputs")]
        public void UpdateCells_ForIncorrectLine_ReturnNull(string initialCells, CrosswordLine line)
        {
            var solver = new CrosswordLineCellsUpdater(GetCellsFromString(initialCells), line);
            var solution = solver.GetUpdatedCells();

            Assert.That(solution, Is.Null);
        }


        private IEnumerable<object[]> GetGoodInputs()
        {

            yield return new object[] { "", new CrosswordLine(), "" };
            yield return new object[] { "??", new CrosswordLine(), "  " };
            yield return new object[] { "??", new CrosswordLine(2), "**" };
            yield return new object[] { "*??  ??", new CrosswordLine(3, 1), "***  ??" };
            yield return new object[] { "?***?", new CrosswordLine(3), " *** " };
            yield return new object[] { "????", new CrosswordLine(3), "?**?" };
            yield return new object[] { "***?", new CrosswordLine(3), "*** " };
            yield return new object[] { "**? ?", new CrosswordLine(2, 1), "**  *" };
            yield return new object[] { " **??", new CrosswordLine(3), " *** " };
            yield return new object[] { "??????????", new CrosswordLine(1, 3, 2), "????*?????" };
            yield return new object[] { "??????????", new CrosswordLine(1, 4, 2), "???***??*?" };
            yield return new object[] { "*? *??????", new CrosswordLine(1, 3, 2), "*  *** ?*?" };
            yield return new object[] { "????????????", new CrosswordLine(1, 1, 2, 4), "?????*??***?" };
            yield return new object[] { "???*??", new CrosswordLine(1, 1, 1), "?? * *" };
            yield return new object[] { new string('?', 100), new CrosswordLine(1, 1, 1, 1), new string('?', 100) };
            yield return new object[] { "?????*???*?", new CrosswordLine(1, 1, 1, 2), "???? * ??*?" };
            yield return new object[] { " ?????? ???*?", new CrosswordLine(2, 2, 2, 2), " ?*??*? ** **" };
        }

        private static IEnumerable<object[]> GetBadInputs()
        {

            yield return new object[] { "", new CrosswordLine(2, 1) };
            yield return new object[] { "* ?", new CrosswordLine(1, 2) };
            yield return new object[] { "**", new CrosswordLine(1) };
            yield return new object[] { "*??????*", new CrosswordLine(2, 2, 3) };
            yield return new object[] { "?????*???*?*", new CrosswordLine(1, 1, 1, 2) };
            yield return new object[] { "****?", new CrosswordLine(4, 1) };

        }

        private static List<CrosswordCell> GetCellsFromString(string cells)
        {
            return cells.Select(GetCellFromChar).ToList();
        }

        private static CrosswordCell GetCellFromChar(char c)
        {
            switch (c)
            {
                case ' ':
                    return CrosswordCell.Empty;
                case '*':
                    return CrosswordCell.Filled;
                case '?':
                    return CrosswordCell.Unclear;
                default:
                    throw new ArgumentException();

            }
        }
    }
}