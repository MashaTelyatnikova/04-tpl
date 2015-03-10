using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils.CrosswordTemplateBuilderUtils;
using JapaneseCrossword.CrosswordUtils.CrosswordTemplateUtils.CrosswordTemplateBuilderUtils.Interfaces;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordTemplateBuilderTests
    {
        private ICrosswordTemplateBuilder crosswordTemplateBuilder;

        [SetUp]
        public void SetUp()
        {
            crosswordTemplateBuilder = new CrosswordTemplateBuilder();
        }

        [TestCase("lalal.txt")]
        [TestCase("lala")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase(@"CrosswordTemplateBuilder.TestFiles\Incorrect1.txt")]
        [TestCase(@"CrosswordTemplateBuilder.TestFiles\Incorrect2.txt")]
        public void BuildFromFile_ForIncorrectFile_ThrowsArgumentException(string file)
        {
            Assert.Throws<ArgumentException>(() => crosswordTemplateBuilder.BuildFromFile(file));
        }

        [Test]
        public void BuildFromFile_ForEmptyFile_ReturnEmptyTemplate()
        {
            var file = @"CrosswordTemplateBuilder.TestFiles\Empty.txt";

            var actualCrosswordTemplate = crosswordTemplateBuilder.BuildFromFile(file);
            var expectedCrosswordTemplate = new CrosswordTemplate(Enumerable.Empty<CrosswordTemplateRow>(),
                Enumerable.Empty<CrosswordTemplateColumn>());

            Assert.That(actualCrosswordTemplate, Is.EqualTo(expectedCrosswordTemplate));
        }

        [Test]
        public void BuildFromFile_ForTemplateWithSimpleRowsAndColumns_ReturnCorrectResult()
        {
            var file = @"CrosswordTemplateBuilder.TestFiles\Simple.txt";

            var actualCrosswordTemplate = crosswordTemplateBuilder.BuildFromFile(file);
            var expectedRows = new List<CrosswordTemplateRow>() { new CrosswordTemplateRow(1) };
            var expectedColumns = new List<CrosswordTemplateColumn>() { new CrosswordTemplateColumn(1) };

            var expectedCrosswordTemplate = new CrosswordTemplate(expectedRows, expectedColumns);

            Assert.That(actualCrosswordTemplate, Is.EqualTo(expectedCrosswordTemplate));
        }

        [Test]
        public void BuildFromFile_ForTemplateWithComplexRowsAndColumns_ReturnCorrectResult()
        {
            var file = @"CrosswordTemplateBuilder.TestFiles\Complex.txt";

            var actualCrosswordTemplate = crosswordTemplateBuilder.BuildFromFile(file);
            var expectedRows = new List<CrosswordTemplateRow>()
            { 
                new CrosswordTemplateRow(1,2,3),
                new CrosswordTemplateRow(1), 
                new CrosswordTemplateRow(1, 4), 
                new CrosswordTemplateRow(1,2,5)
            };

            var expectedColumns = new List<CrosswordTemplateColumn>()
            {
                new CrosswordTemplateColumn(2, 2), new CrosswordTemplateColumn(1, 3)
            };

            var expectedCrosswordTemplate = new CrosswordTemplate(expectedRows, expectedColumns);

            Assert.That(actualCrosswordTemplate, Is.EqualTo(expectedCrosswordTemplate));
        }
    }
}
