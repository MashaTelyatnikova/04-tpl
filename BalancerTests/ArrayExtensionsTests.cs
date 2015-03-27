using Balancer;
using NUnit.Framework;

namespace BalancerTests
{
    [TestFixture]
    public class ArrayExtensionsTests
    {
        [TestCase(new []{1, 2, 3})]
        [TestCase(new int[0])]
        [TestCase(new []{1})]
        public void Shuffle_ForRandomArrayReturnsCorrectResult(int[] array)
        {
            var mixedArray = array.Shuffle();
            Assert.That(mixedArray, Is.EquivalentTo(array));
        }
    }
}
