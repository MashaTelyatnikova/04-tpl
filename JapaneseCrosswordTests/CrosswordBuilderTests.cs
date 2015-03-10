using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilder;
using JapaneseCrossword.CrosswordUtils.CrosswordBuilder.Interfaces;
using JapaneseCrossword.CrosswordUtils.Enums;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordBuilderTests
    {
        private ICrosswordBuilder crosswordBuilder;

        [SetUp]
        public void SetUp()
        {
            crosswordBuilder = new CrosswordBuilder();
        }

        [TestCase("lalal.txt")]
        [TestCase("lala")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase(@"CrosswordBuilder.TestFiles\Incorrect1.txt")]
        [TestCase(@"CrosswordBuilder.TestFiles\Incorrect2.txt")]
        public void BuildFromFile_ForIncorrectFile_ThrowsArgumentException(string file)
        {
            Assert.Throws<ArgumentException>(() => crosswordBuilder.BuildFromFile(file));
        }

        [Test]
        public void BuildFromFile_ForEmptyFile_ReturnEmptyTemplate()
        {
            var file = @"CrosswordBuilder.TestFiles\Empty.txt";

            var actualCrossword = crosswordBuilder.BuildFromFile(file);
            var expectedCrossword = new Crossword(0, 0, Enumerable.Empty<CrosswordLine>());
            Assert.That(actualCrossword, Is.EqualTo(expectedCrossword));
        }

        [Test]
        public void BuildFromFile_ForTemplateWithSimpleRowsAndColumns_ReturnCorrectResult()
        {
            var file = @"CrosswordBuilder.TestFiles\Simple.txt";

            var actualCrossword = crosswordBuilder.BuildFromFile(file);
            var expectedCrossword = new Crossword(1, 1,
                new List<CrosswordLine>
                {
                    new CrosswordLine(0, CrosswordLineType.Row, 1),
                    new CrosswordLine(0, CrosswordLineType.Column, 1)
                });

            Assert.That(actualCrossword, Is.EqualTo(expectedCrossword));
        }

        [Test]
        public void BuildFromFile_ForTemplateWithComplexRowsAndColumns_ReturnCorrectResult()
        {
            var file = @"CrosswordBuilder.TestFiles\Complex.txt";

            var actualCrossword = crosswordBuilder.BuildFromFile(file);

            var expectedCrossword = new Crossword(2, 4, new List<CrosswordLine>()
            {
                new CrosswordLine(0, CrosswordLineType.Row, 1, 2, 3),
                new CrosswordLine(1, CrosswordLineType.Row, 1),
                new CrosswordLine(2, CrosswordLineType.Row, 1, 4),
                new CrosswordLine(3, CrosswordLineType.Row, 1, 2, 5),
                new CrosswordLine(0, CrosswordLineType.Column, 2, 2),
                new CrosswordLine(1, CrosswordLineType.Column, 1, 3)
            });
            
            Assert.That(actualCrossword, Is.EqualTo(expectedCrossword));
        }
    }
}
