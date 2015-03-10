using System;
using System.Collections.Generic;
using System.Linq;
using JapaneseCrossword.CrosswordUtils;
using JapaneseCrossword.CrosswordUtils.Enums;
using NUnit.Framework;

namespace JapaneseCrosswordTests
{
    [TestFixture]
    public class CrosswordTests
    {
        [Test]
        public void Test1()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new Crossword(null); });
        }

        [Test]
        public void Test2()
        {
            var crossword = new Crossword(Enumerable.Empty<CrosswordLine>());
            
            Assert.That(crossword.Width, Is.EqualTo(0));
            Assert.That(crossword.Height, Is.EqualTo(0));
        }

        [Test]
        public void Test3()
        {
            var crossword = new Crossword(new List<CrosswordLine>()
            {
                new CrosswordLine(0, CrosswordLineType.Row),
                new CrosswordLine(1, CrosswordLineType.Row),
                new CrosswordLine(0, CrosswordLineType.Column),
                new CrosswordLine(1, CrosswordLineType.Column)
            });

            Assert.That(crossword.Width, Is.EqualTo(2));
            Assert.That(crossword.Height, Is.EqualTo(2));
        }

        [Test]
        public void Test4()
        {
            var crossword = new Crossword(new List<CrosswordLine>()
            {
                new CrosswordLine(0, CrosswordLineType.Row),
                new CrosswordLine(1, CrosswordLineType.Row),
                new CrosswordLine(0, CrosswordLineType.Column),
                new CrosswordLine(1, CrosswordLineType.Column),
                new CrosswordLine(2, CrosswordLineType.Column)
            });

            Assert.That(crossword.Width, Is.EqualTo(3));
            Assert.That(crossword.Height, Is.EqualTo(2));
        }

        [Test]
        public void Test5()
        {
            var crossword = new Crossword(
                new List<CrosswordLine>()
                {
                    new CrosswordLine(0, CrosswordLineType.Row),
                    new CrosswordLine(1, CrosswordLineType.Row),
                    new CrosswordLine(0, CrosswordLineType.Column)
                });

           Assert.That(crossword.GetLine(0, CrosswordLineType.Column), Is.EqualTo(new CrosswordLine(0, CrosswordLineType.Column)));
           Assert.That(crossword.GetLine(1, CrosswordLineType.Row), Is.EqualTo(new CrosswordLine(1, CrosswordLineType.Row)));
           Assert.That(crossword.GetLine(1, CrosswordLineType.Column), Is.Null);

        }
    }
}
