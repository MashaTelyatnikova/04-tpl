using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace Balancer
{
    public class Listener
    {
        private readonly HttpListener listener;
        private Func<HttpListenerContext, Task> CallbackAsync { get; set; }

        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        public Listener(int port, string suffix, Func<HttpListenerContext, Task> callbackAsync)
        {
            ThreadPool.SetMinThreads(32, 32);
            CallbackAsync = callbackAsync;
            listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://+:{0}{1}/", port, suffix != null ? "/" + suffix.TrimStart('/') : ""));
        }

        public void Start()
        {
            listener.Start();
            StartListen();
        }

        public async void StartListen()
        {
            while (true)
            {
                try
                {
                    var context = await listener.GetContextAsync();

                    Task.Run(
                        async () =>
                        {
                            var ctx = context;
                            try
                            {
                                await CallbackAsync(ctx);
                            }
                            catch (Exception e)
                            {
                                Log.Error(e);
                            }
                            finally
                            {
                                ctx.Response.Close();
                            }
                        }
                    );
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}
