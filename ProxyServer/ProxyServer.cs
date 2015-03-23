﻿using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyServer
{
    public class ProxyServer
    {
        private readonly Listener listener;
        private readonly ConcurrentDictionary<string, string> cache;
        private readonly string[] serversReplicas;
        private const int Timeout = 2000;
        private readonly GrayList grayList;

        public ProxyServer(Settings settings)
        {
            serversReplicas = settings.ServersReplicas;
            grayList = new GrayList(settings.ResidenceTimeInGrayList);
            listener = new Listener(settings.Port, null, HandleRequest);
            cache = new ConcurrentDictionary<string, string>();
        }

        public void Run()
        {
            try
            {
                listener.Start();

                new ManualResetEvent(false).WaitOne();
            }
            catch
            {
                //
            }
        }

        private async Task HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var query = request.Url.Query;
            string result;
            if (!cache.ContainsKey(query))
            {
                var answer = await GetAnswerFromReplica(query);
                if (answer != null)
                {
                    cache.TryAdd(query, answer);
                }
            }

            if (cache.TryGetValue(query, out result))
            {
                SendResponse(result, 200, context);
            }
            else
            {
                SendResponse("Not Found =(", 404, context);
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

        private async Task<string> GetAnswerFromReplica(string query)
        {
            var mixedReplicas = serversReplicas.Where(replica => !grayList.ContainsRecord(replica)).ToArray().Shuffle();

            foreach (var replica in mixedReplicas)
            {
                var answer = await GetAnswerAsync(replica + query);
                if (answer != null)
                    return answer;
                grayList.AddRecord(replica);
            }

            return null;
        }

        private static async Task<string> GetAnswerAsync(string url)
        {
            try
            {
                var webRequest = WebRequest.Create(url);
                webRequest.Timeout = Timeout;
                
                var response = await webRequest.GetResponseAsync();
                
                var responseStream = response.GetResponseStream();
                var answer =  new StreamReader(responseStream).ReadToEnd();

                return answer;
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}