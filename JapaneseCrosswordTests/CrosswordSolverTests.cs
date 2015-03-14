using System.Collections.Generic;
using System.IO;
using System.Linq;
using JapaneseCrossword;
using JapaneseCrossword.CrosswordBuilderUtils;
using JapaneseCrossword.CrosswordSolutionUtils;
using JapaneseCrossword.CrosswordSolverUtils;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordSolverTests
    {
        private ICrosswordBuilder builder;
        private List<ICrosswordSolver> crosswordSolvers;
        private ICrosswordSolutionVisualizer<string> crosswordSolutionVisualizer;

        [SetUp]
        public void SetUp()
        {
            builder = new CrosswordBuilder();
            crosswordSolutionVisualizer = new CrosswordSolutionTextVisualizer();
            crosswordSolvers = new List<ICrosswordSolver>() { new SingleThreadedCrosswordSolver(), new MultiThreadedCrosswordSolver() };
        }

        [Test]
        public void Solve_ForIncorrectCrossword_ReturnCorrectSolution()
        {
            var crosswordTemplate = builder.BuildFromFile(@"CrosswordSolver.TestFiles\IncorrectCrossword.txt");


            foreach (var actualSolution in crosswordSolvers.Select(solver => solver.Solve(crosswordTemplate)))
            {
                Assert.That(crosswordSolutionVisualizer.Visualize(actualSolution), Is.EqualTo(""));
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
                Assert.That(crosswordSolutionVisualizer.Visualize(actualSolution), Is.EqualTo(expectedSolution));
                Assert.That(actualSolution.Status, Is.EqualTo(CrosswordSolutionStatus.Solved));
            }
        }

        [Test]
        public void WinterTest()
        {
            var crosswordTemplate = builder.BuildFromFile(@"CrosswordSolver.TestFiles\Winter.txt");
            var expectedSolution = GetNormalTextFromFile(@"CrosswordSolver.TestFiles\Winter.Solved.txt");
            foreach (var actualSolution in crosswordSolvers.Select(solver => solver.Solve(crosswordTemplate)))
            {
                Assert.That(crosswordSolutionVisualizer.Visualize(actualSolution), Is.EqualTo(expectedSolution));
                Assert.That(actualSolution.Status, Is.EqualTo(CrosswordSolutionStatus.PartiallySolved));
            }
        }

        [TestCase(1, 10, 1, 10)]
        [TestCase(5, 15, 5, 15)]
        [TestCase(12, 13, 12, 13)]
        public void RandomCrosswordsTest(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var crosswordGenerator = new CrosswordGenerator();
            for (var i = 0; i < 1000; ++i)
            {
                var randomCrossword = crosswordGenerator.Next(minWidth, maxWidth, minHeight, maxHeight);
                var crossword = randomCrossword.Item1;
                var answer = randomCrossword.Item2;
                foreach (var actualSolution in crosswordSolvers.Select(solver => solver.Solve(crossword)))
                {
                    Assert.That(Check(answer, crosswordSolutionVisualizer.Visualize(actualSolution)), Is.True);
                }
            }
        }

        private static bool Check(string generatorAnswer, string crosswordSolverAnswer)
        {
            var answerLines = generatorAnswer.Split('\n').ToList();
            var actualLines = crosswordSolverAnswer.Split('\n').ToList();

            return !answerLines.Where((t, i) => !CheckLine(t, actualLines[i])).Any();
        }

        private static bool CheckLine(string generatorAnswer, string crosswordSolverAnswer)
        {
            return !generatorAnswer.Where((t, i) => t != crosswordSolverAnswer[i] && crosswordSolverAnswer[i] != '?')
                                    .Any();
        }

        private static string GetNormalTextFromFile(string fileName)
        {
            return File.ReadAllText(fileName).Replace("\r", "").Trim();
        }
    }
}