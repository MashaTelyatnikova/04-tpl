using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils.CrosswordTemplateBuilderUtils;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordSolverTests
    {
        private ICrosswordTemplateBuilder builder;

        [SetUp]
        public void SetUp()
        {
            builder = new CrosswordTemplateBuilder();
        }

        [Test]
        public void Solve_ForIncorrectCrossword_ReturnCorrectSolution()
        {
            var crosword = new CrosswordSolver(builder.BuildFromFile(@"CrosswordSolver.TestFiles\IncorrectCrossword.txt"));
            var actualSolution = crosword.Solve();
            var expectedSolution = new CrosswordSolution(Enumerable.Empty<List<CrosswordSolutionCell>>().ToList(),
                SolutionStatus.IncorrectCrossword);

            Assert.That(actualSolution, Is.EqualTo(expectedSolution));
        }

        [Test]
        public void Solve_ForSimplyRightCrossword_ReturnCorrectSolution()
        {
            var crosword = new CrosswordSolver(builder.BuildFromFile(@"CrosswordSolver.TestFiles\SampleInput.txt"));
            var actualSolution = crosword.Solve();

            var expectedSolution = new CrosswordSolution(
                new List<List<CrosswordSolutionCell>>()
                {
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled}
                }, SolutionStatus.Solved);

            Assert.That(actualSolution, Is.EqualTo(expectedSolution));
        }

        [Test]
        public void Solve_ForCrosswordDescribingCar_ReturnCorrectSolution()
        {
            var crosword = new CrosswordSolver(builder.BuildFromFile(@"CrosswordSolver.TestFiles\Car.txt"));
            var actualSolution = crosword.Solve();
            var expectedSolution = new CrosswordSolution(
                new List<List<CrosswordSolutionCell>>()
                {
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Filled, CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Empty},
                    new List<CrosswordSolutionCell>(){CrosswordSolutionCell.Empty, CrosswordSolutionCell.Empty,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Empty,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Filled,CrosswordSolutionCell.Empty}
                }, SolutionStatus.Solved);

            Assert.That(actualSolution, Is.EqualTo(expectedSolution));
        }
    }
}
