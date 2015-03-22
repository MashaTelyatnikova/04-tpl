using System.IO;
using System.IO.Compression;

namespace ProxyServer
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
