using System.IO;
using System.IO.Compression;
using System.Text;

namespace Balancer
{
    public static class ByteArrayExtensions
    {
        public static byte[] CompressByDeflate(this byte[] source)
        {
            byte[] result;
            using (var memory = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(memory, CompressionMode.Compress, true))
                {
                    deflateStream.Write(source, 0, source.Length);
                }

                result = memory.ToArray();
            }

            return result;
        }

        public static byte[] DecompressByDeflate(this byte[] source)
        {
            using (var memory = new MemoryStream(source))
            {
                using (var deflateStream = new DeflateStream(memory, CompressionMode.Decompress, true))
                {
                    using (var reader = new StreamReader(deflateStream, System.Text.Encoding.UTF8))
                    {
                        return Encoding.UTF8.GetBytes(reader.ReadToEnd());
                    }
                }
            }
        }

        public static byte[] CompressByGzip(this byte[] source)
        {
            byte[] result;
            using (var memory = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gZipStream.Write(source, 0, source.Length);
                }

                result = memory.ToArray();
            }

            return result;
        }
    }
}
