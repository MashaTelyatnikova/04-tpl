using System.Linq;
using System.Net;

namespace Balancer
{
    public static class HttpListenerRequestExtensions
    {
        public static bool ClientSupportsGzipCompression(this HttpListenerRequest request)
        {
            if (!request.Headers.AllKeys.Contains("Accept-Encoding"))
            {
                return false;
            }

            var encodings = request.Headers["Accept-Encoding"].Split(',').Select(encoding => encoding.Trim());

            return encodings.Contains("gzip");
        }

        public static bool ClientSupportsDeflateCompression(this HttpListenerRequest request)
        {
            if (!request.Headers.AllKeys.Contains("Accept-Encoding"))
            {
                return false;
            }

            var encodings = request.Headers["Accept-Encoding"].Split(',').Select(encoding => encoding.Trim());

            return encodings.Contains("deflate");
        }
    }
}
