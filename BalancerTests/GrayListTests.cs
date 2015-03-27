using System.Threading;
using Balancer;
using NUnit.Framework;

namespace BalancerTests
{
    [TestFixture]
    public class GrayListTests
    {
        [Test]
        public void ContainsRecord_ForZeroTimeout_AlwaysReturnsFalse()
        {
            var list = new GrayList(0);
            list.AddRecord("localhost");
            
            Assert.That(list.ContainsRecord("localhost"), Is.False);
        }

        [Test]
        public void ContainsRecord_ForSecondTimeoutAndOneRecord_ReturnsCorrectResult()
        {
            var second = 1/60.0;
            var list = new GrayList(second);
            list.AddRecord("localhost");
            Assert.That(list.ContainsRecord("localhost"), Is.True);
            Thread.Sleep(1000);
            Assert.That(list.ContainsRecord("localhost"), Is.False);
        }

        [Test]
        public void ContainsRecord_ForSecondTimeoutAndFewRecords_ReturnsCorrectResult()
        {
            var second = 1 / 60.0;
            var list = new GrayList(second);
            list.AddRecord("localhost1");
            Thread.Sleep(200);
            list.AddRecord("localhost2");
            
            Assert.That(list.ContainsRecord("localhost1"), Is.True);
            Assert.That(list.ContainsRecord("localhost2"), Is.True);

            Thread.Sleep(800);
            Assert.That(list.ContainsRecord("localhost1"), Is.False);
            Assert.That(list.ContainsRecord("localhost2"), Is.True);

            Thread.Sleep(200);
            Assert.That(list.ContainsRecord("localhost1"), Is.False);
            Assert.That(list.ContainsRecord("localhost2"), Is.False);

        }

        [Test]
        public void RemoveRecord_ForSecondTimeoutAndFewRecords_ReturnsCorrectResult()
        {
            var second = 1 / 60.0;
            var list = new GrayList(second);
            list.AddRecord("localhost1");
            Thread.Sleep(200);
            list.AddRecord("localhost2");

            Assert.That(list.ContainsRecord("localhost1"), Is.True);
            Assert.That(list.ContainsRecord("localhost2"), Is.True);

            list.RemoveRecord("localhost1");
            Assert.That(list.ContainsRecord("localhost1"), Is.False);
            Assert.That(list.ContainsRecord("localhost2"), Is.True);

            Thread.Sleep(1000);
            Assert.That(list.ContainsRecord("localhost1"), Is.False);
            Assert.That(list.ContainsRecord("localhost2"), Is.False);

        }
    }
}
