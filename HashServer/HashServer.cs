using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace HashServerUtils
{
    public class HashServer
    {
        private Listener listener;
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("Контур.Шпора");
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private int port;
        public HashServer(int port)
        {
            this.port = port;
            listener = new Listener(port, "method", OnContextAsync);
        }

        public void Start()
        {
            listener.Start();    
        }

        public void Restart()
        {
            listener.Stop();
            listener = new Listener(port, "method", OnContextAsync);
            listener.Start();
        }

        public void Stop()
        {
            listener.Stop();
        }

        private static async Task OnContextAsync(HttpListenerContext context)
        {
            var requestId = Guid.NewGuid();
            var query = context.Request.QueryString["query"];
            var remoteEndPoint = context.Request.RemoteEndPoint;
            log.InfoFormat("{0}: received {1} from {2}", requestId, query, remoteEndPoint);
            context.Request.InputStream.Close();

            await Task.Delay(1000);
            //			Thread.Sleep(1000);

            var encryptedBytes = Encoding.UTF8.GetBytes(query);
            await context.Response.OutputStream.WriteAsync(encryptedBytes, 0, encryptedBytes.Length);
            context.Response.OutputStream.Close();
        }

        private static byte[] CalcHash(byte[] data)
        {
            using (var hasher = new HMACMD5(Key))
                return hasher.ComputeHash(data);
        }
    }
}
