using System.Collections.Generic;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils.Enums;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordSolutionTests
    {
        [Test]
        public void Test1()
        {
            var crossword = new CrosswordSolution(new List<List<CrosswordSolutionCell>>(), SolutionStatus.Solved);
            Assert.That(crossword.Width, Is.EqualTo(0));
            Assert.That(crossword.Height, Is.EqualTo(0));
            Assert.That(crossword.Status, Is.EqualTo(SolutionStatus.Solved));
            Assert.That(crossword.CrosswordCells, Is.EqualTo(new List<List<CrosswordSolutionCell>>()));
        }

        [Test]
        public void Test2()
        {
            var crossword = new CrosswordSolution(new List<List<CrosswordSolutionCell>>()
            {
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled}
            }, SolutionStatus.Solved);

            Assert.That(crossword.Width, Is.EqualTo(1));
            Assert.That(crossword.Height, Is.EqualTo(2));
            Assert.That(crossword.Status, Is.EqualTo(SolutionStatus.Solved));
            Assert.That(crossword.CrosswordCells, Is.EqualTo(new List<List<CrosswordSolutionCell>>(){
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled}
            }));
        }

        [Test]
        public void Test3()
        {
            var firstCrosswordSolution = new CrosswordSolution(new List<List<CrosswordSolutionCell>>()
            {
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled}
            }, SolutionStatus.Solved);
            var secondCrosswordSolution = new CrosswordSolution(new List<List<CrosswordSolutionCell>>()
            {
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled}
            }, SolutionStatus.Solved);
            Assert.AreEqual(firstCrosswordSolution, secondCrosswordSolution);
        }

        [Test]
        public void Test4()
        {
            var firstCrosswordSolution = new CrosswordSolution(new List<List<CrosswordSolutionCell>>()
            {
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled}
            }, SolutionStatus.Solved);
            var secondCrosswordSolution = new CrosswordSolution(new List<List<CrosswordSolutionCell>>()
            {
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}
            }, SolutionStatus.Solved);
            Assert.AreNotEqual(firstCrosswordSolution, secondCrosswordSolution);
        }

        [Test]
        public void Test5()
        {
            var firstCrosswordSolution = new CrosswordSolution(new List<List<CrosswordSolutionCell>>()
            {
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled}
            }, SolutionStatus.PartiallySolved);
            var secondCrosswordSolution = new CrosswordSolution(new List<List<CrosswordSolutionCell>>()
            {
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty}, 
                new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled}
            }, SolutionStatus.Solved);
            Assert.AreNotEqual(firstCrosswordSolution, secondCrosswordSolution);
        }
    }
}
