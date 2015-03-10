using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils.Enums;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordLineCellsUpdaterTests
    {
        [Test]
        public void UpdateCells_ForEmptyParameters_ReturnEmptyResult()
        {
            var updater =
                new CrosswordLineCellsUpdater(
                   Enumerable.Empty<CrosswordSolutionCell>(), new int[] { });

            var actualUpdatedCells = updater.UpdateCells();
            var expectedUpdatedCells = Enumerable.Empty<CrosswordSolutionCell>();

            Assert.That(expectedUpdatedCells, Is.EqualTo(actualUpdatedCells));
        }

        [Test]
        public void UpdateCells_ForEmptyBlocks_ReturnCorrectResult()
        {
            var updater =
                new CrosswordLineCellsUpdater(
                    new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Unclear, CrosswordSolutionCell.Unclear },
                    new int[] { });

            var actualUpdatedCells = updater.UpdateCells();
            var expectedUpdatedCells = new List<CrosswordSolutionCell>()
            {
                CrosswordSolutionCell.Empty,
                CrosswordSolutionCell.Empty
            };

            Assert.That(expectedUpdatedCells, Is.EqualTo(actualUpdatedCells));
        }

        [Test]
        public void UpdateCells_ForOneBlockFitsInLine_ReturnCorrectResult()
        {
            var updater =
               new CrosswordLineCellsUpdater(
                   new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Unclear, CrosswordSolutionCell.Unclear },
                   new[] { 2 });

            var actualUpdatedCells = updater.UpdateCells();
            var expectedUpdatedCells = new List<CrosswordSolutionCell>()
            {
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Filled
            };

            Assert.That(expectedUpdatedCells, Is.EqualTo(actualUpdatedCells));
        }

        [Test]
        public void UpdateCells_ForBlocksThatDoNotFitIntoLine_ThrowsArgumentException()
        {
            var updater =
               new CrosswordLineCellsUpdater(
                   new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Filled, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Unclear },
                   new[] { 1, 2 });

            Assert.Throws<ArgumentException>(() => updater.UpdateCells());
        }

        [Test]
        public void Test5()
        {
            var updater =
               new CrosswordLineCellsUpdater(
                   new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled },
                   new[] { 1 });

            Assert.Throws<ArgumentException>(() => updater.UpdateCells());
        }

        [Test]
        public void Test6()
        {
            var updater =
               new CrosswordLineCellsUpdater(
                   new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Unclear, CrosswordSolutionCell.Unclear,
                       CrosswordSolutionCell.Unclear, CrosswordSolutionCell.Unclear },
                   new[] { 3 });

            var actualUpdatedCells = updater.UpdateCells();
            var expectedUpdatedCells = new List<CrosswordSolutionCell>()
            {
                CrosswordSolutionCell.Unclear,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Unclear
            };

            Assert.That(expectedUpdatedCells, Is.EqualTo(actualUpdatedCells));
        }

        [Test]
        public void Test7()
        {
            var updater =
               new CrosswordLineCellsUpdater(
                   new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Unclear },
                   new[] { 3 });

            var actualUpdatedCells = updater.UpdateCells();
            var expectedUpdatedCells = new List<CrosswordSolutionCell>()
            {
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Empty
            };

            Assert.That(expectedUpdatedCells, Is.EqualTo(actualUpdatedCells));
        }

        [Test]
        public void Test8()
        {
            var updater =
               new CrosswordLineCellsUpdater(
                   new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Unclear, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Unclear },
                   new[] { 2, 1 });

            var actualUpdatedCells = updater.UpdateCells();
            var expectedUpdatedCells = new List<CrosswordSolutionCell>()
            {
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Empty,
                CrosswordSolutionCell.Empty,
                CrosswordSolutionCell.Filled
            };

            Assert.That(expectedUpdatedCells, Is.EqualTo(actualUpdatedCells));
        }

        [Test]
        public void Test9()
        {
            var updater =
               new CrosswordLineCellsUpdater(
                   new List<CrosswordSolutionCell>() { CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Unclear, CrosswordSolutionCell.Unclear },
                   new[] {3 });

            var actualUpdatedCells = updater.UpdateCells();
            var expectedUpdatedCells = new List<CrosswordSolutionCell>()
            {
                CrosswordSolutionCell.Empty,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Filled,
                CrosswordSolutionCell.Empty
            };

            Assert.That(expectedUpdatedCells, Is.EqualTo(actualUpdatedCells));
        }
    }
}
