using System.Collections.Generic;
using System.IO;
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

        [TestCase(@"CrosswordSolver.TestFiles\SampleInput.txt", @"CrosswordSolver.TestFiles\SampleInput.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Car.txt", @"CrosswordSolver.TestFiles\Car.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Rabbit.txt", @"CrosswordSolver.TestFiles\Rabbit.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Kettle.txt", @"CrosswordSolver.TestFiles\Kettle.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Heart.txt", @"CrosswordSolver.TestFiles\Heart.Solved.txt")]
        [TestCase(@"CrosswordSolver.TestFiles\Flower.txt", @"CrosswordSolver.TestFiles\Flower.Solved.txt")]
        public void Solve_ForCorrectUnambiguousCrossword_ReturnCorrectSolution(string templateFile, string solutionFile)
        {
            var crossword = new CrosswordSolver(builder.BuildFromFile(templateFile));
            var expectedSolution = GetNormalTextFromFile(solutionFile);
            var actualSolution = crossword.Solve();

            Assert.That(actualSolution.Status, Is.EqualTo(SolutionStatus.Solved));
            Assert.That(actualSolution.ToString(), Is.EqualTo(expectedSolution));
        }

        [Test]
        public void WinterTest()
        {
            var crossword = new CrosswordSolver(builder.BuildFromFile(@"CrosswordSolver.TestFiles\Winter.txt"));
            var expectedSolution = GetNormalTextFromFile(@"CrosswordSolver.TestFiles\Winter.Solved.txt");
            var actualSolution = crossword.Solve();

            Assert.That(actualSolution.Status, Is.EqualTo(SolutionStatus.PartiallySolved));
            Assert.That(actualSolution.ToString(), Is.EqualTo(expectedSolution));
        }
        private static string GetNormalTextFromFile(string fileName)
        {
            return File.ReadAllText(fileName).Replace("\r", "").Trim();
        }
    }
}
