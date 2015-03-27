using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Balancer
{
    public class Balancer
    {
        private readonly Listener listener;
        private readonly ConcurrentDictionary<string, string> cache;
        private readonly string[] serversReplicas;
        private readonly int replicaAnswerTimeout;
        private const string Suffix = "method";
        private const string ErrorMessage = "Error";
        private readonly GrayList grayList;
        private const int ErrorStatusCode = 500;
        private const int SuccessStatusCode = 200;

        public Balancer(Settings settings)
        {
            replicaAnswerTimeout = settings.ReplicaAnswerTimeout;
            serversReplicas = settings.ServersReplicas;
            grayList = new GrayList(settings.ResidenceTimeInGrayList);
            listener = new Listener(settings.Port, Suffix, HandleRequest);
            cache = new ConcurrentDictionary<string, string>();
        }

        public void Run()
        {
            try
            {
                listener.Start();
            }
            catch
            {
                //
            }
        }

        public void Stop()
        {
            listener.Stop();
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var query = request.Url.Query;
            string result;
            if (!cache.ContainsKey(query))
            {
                var answer = await GetAnswerFromReplica(request.Url);
                if (answer != null)
                {
                    cache.TryAdd(query, answer);
                }
            }

            if (cache.TryGetValue(query, out result))
            {
                SendResponse(result, SuccessStatusCode, context);
            }
            else
            {
                SendResponse(ErrorMessage, ErrorStatusCode, context);
            }
        }

        private static async void SendResponse(string response, int statusCode, HttpListenerContext context)
        {
            try
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);
                if (context.Request.ClientSupportsGzipCompression())
                {
                    context.Response.Headers.Add("Content-Encoding", "gzip");
                    responseBytes = responseBytes.CompressByGzip();
                }
                else if (context.Request.ClientSupportsDeflateCompression())
                {
                    context.Response.Headers.Add("Content-Encoding", "deflate");
                    responseBytes = responseBytes.CompressByDeflate();
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentLength64 = responseBytes.Length;
                await context.Response.OutputStream.WriteAsync(responseBytes, 0, responseBytes.Count());
            }
            finally
            {
                context.Response.Close();
            }
        }

        private async Task<string> GetAnswerFromReplica(Uri url)
        {
            var workingReplicas = serversReplicas.Where(replica => !grayList.ContainsRecord(replica))
                                                    .ToArray();
            if (workingReplicas.Length == 0)
            {
                workingReplicas = serversReplicas;
            }

            var mixedWorkingReplicas = workingReplicas.Shuffle();

            foreach (var replica in mixedWorkingReplicas)
            {
                var answer = await GetAnswerAsync(replica + url.AbsolutePath + url.Query);
                if (answer != null)
                {
                    if (grayList.ContainsRecord(replica))
                    {
                        grayList.RemoveRecord(replica);
                    }
                    return answer;
                }

                grayList.AddRecord(replica);
            }

            return null;
        }

        private async Task<string> GetAnswerAsync(string url)
        {
            try
            {
                var webRequest = WebRequest.Create(url);
                webRequest.Timeout = replicaAnswerTimeout;

                using (var response = await webRequest.GetResponseAsync())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        var answer = new StreamReader(responseStream).ReadToEnd();

                        return answer;
                    }
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}
