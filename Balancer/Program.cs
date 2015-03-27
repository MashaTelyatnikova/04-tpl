using System;
using System.Threading;

namespace Balancer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var settings = new Settings("settings.txt");

            var balancer = new Balancer(settings);
            balancer.Run();
            new ManualResetEvent(false).WaitOne();
        }
    }
}
