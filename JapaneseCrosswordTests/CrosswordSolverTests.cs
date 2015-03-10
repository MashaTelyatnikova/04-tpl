using System.Collections.Generic;
using System.IO;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilder;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilder.Interfaces;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolutionUtils.Enums;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordSolverUtils.Interfaces;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordSolverTests
    {
        private ICrosswordBuilder builder;
        private List<ICrosswordSolver> crosswordSolvers;
            
        [SetUp]
        public void SetUp()
        {
            builder = new CrosswordBuilder();
            crosswordSolvers = new List<ICrosswordSolver>(){new SingleThreadedCrosswordSolver(), new MultiThreadedCrosswordSolver()};
        }

        [Test]
        public void Solve_ForIncorrectCrossword_ReturnCorrectSolution()
        {
            var crosswordTemplate = builder.BuildFromFile(@"CrosswordSolver.TestFiles\IncorrectCrossword.txt");
            var expectedSolution = new CrosswordSolution(Enumerable.Empty<List<CrosswordSolutionCell>>().ToList(),
                SolutionStatus.IncorrectCrossword);

            foreach (var actualSolution in crosswordSolvers.Select(solver => solver.Solve(crosswordTemplate)))
            {
                Assert.That(actualSolution, Is.EqualTo(expectedSolution));
            }
        }

        [TestCase(@"CrosswordSolver.TestFiles\SampleInput.txt", @"CrosswordSolver.TestFiles\SampleInput.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Car.txt", @"CrosswordSolver.TestFiles\Car.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Rabbit.txt", @"CrosswordSolver.TestFiles\Rabbit.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Kettle.txt", @"CrosswordSolver.TestFiles\Kettle.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Heart.txt", @"CrosswordSolver.TestFiles\Heart.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Flower.txt", @"CrosswordSolver.TestFiles\Flower.Solved.txt")]
        public void Solve_ForCorrectUnambiguousCrossword_ReturnCorrectSolution(string templateFile, string solutionFile)
        {
            var crosswordTemplate = builder.BuildFromFile(templateFile);
            var expectedSolution = GetNormalTextFromFile(solutionFile);
            
            foreach (var actualSolution in crosswordSolvers.Select(solver => solver.Solve(crosswordTemplate)))
            {
                Assert.That(actualSolution.ToString(), Is.EqualTo(expectedSolution));
                Assert.That(actualSolution.Status, Is.EqualTo(SolutionStatus.Solved));
            }
        }

        [Test]
        public void WinterTest()
        {
            var crosswordTemplate = builder.BuildFromFile(@"CrosswordSolver.TestFiles\Winter.txt");
            var expectedSolution = GetNormalTextFromFile(@"CrosswordSolver.TestFiles\Winter.Solved.txt");
            foreach (var actualSolution in crosswordSolvers.Select(solver => solver.Solve(crosswordTemplate)))
            {
                Assert.That(actualSolution.ToString(), Is.EqualTo(expectedSolution));
                Assert.That(actualSolution.Status, Is.EqualTo(SolutionStatus.PartiallySolved));
            }
        }

        private static string GetNormalTextFromFile(string fileName)
        {
            return File.ReadAllText(fileName).Replace("\r", "").Trim();
        }
    }
}
