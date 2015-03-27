using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace HashServerUtils
{
	class Program
	{
		static void Main(string[] args)
		{
			XmlConfigurator.Configure();
			try
			{
			    var port = int.Parse(args[0]);
                Console.WriteLine(port);
				var hashServer = new HashServer(port);
			    hashServer.Start();
				new ManualResetEvent(false).WaitOne();
			}
			catch(Exception e)
			{
				throw;
			}
		}


	}
}