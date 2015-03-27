using Balancer;
using NUnit.Framework;

namespace BalancerTests
{
    [TestFixture]
    public class ByteArrayExtensionsTests
    {
        [Test]
        public void CompressArrayByDeflate_ForRandomArray_ReturnsCorrectResult()
        {
            var array = new byte[] {1, 2, 3};
            var compressedArray = array.CompressByDeflate();

            Assert.That(array, Is.EqualTo(new byte[]{1, 2, 3}));
            Assert.That(array, Is.Not.EqualTo(compressedArray));
            Assert.That(array, Is.EqualTo(compressedArray.DecompressByDeflate()));
        }

        [Test]
        public void CompressArrayByGzip_ForRandomArray_ReturnsCorrectResult()
        {
            var array = new byte[] { 1, 2, 3 };
            var compressedArray = array.CompressByGzip();

            Assert.That(array, Is.EqualTo(new byte[] { 1, 2, 3 }));
            Assert.That(array, Is.Not.EqualTo(compressedArray));
            Assert.That(array, Is.EqualTo(compressedArray.DecompressByGzip()));
        }
    }
}
