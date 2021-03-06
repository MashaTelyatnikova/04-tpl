﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Balancer;
using HashServerUtils;
using MoreLinq;
using NUnit.Framework;

namespace BalancerTests
{
    [TestFixture]
    public class Balancer_should
    {
        [Test]
        public void trying_get_answer_from_each_active_replica_until_it_receives()
        {
            var replicas = new[] { new HashServer(30000), new HashServer(30001), };
            var balancer =
                new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000", "http://127.0.0.1:30001" },
                    1 / 60.0, 3000));

            balancer.Run();
            replicas[1].Start();

            CheckRequest("http://127.0.0.1:20001/method?query=5", "5");


            replicas.ForEach(replica => replica.Stop());
            balancer.Stop();
        }

        [Test]
        public void returns_correct_responses_for_correct_random_requests()
        {
            var replica = new HashServer(30000);
            var balancer = new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000" }, 1 / 60.0, 3000));

            var currentTime = DateTime.Now;

            balancer.Run();
            replica.Start();
            var random = new Random();
            while (DateTime.Now.Subtract(currentTime).TotalSeconds < 2)
            {
                var query = random.Next(0, 100).ToString();
                CheckRequest("http://127.0.0.1:20001/method?query=" + query, query);
            }
            balancer.Stop();
            replica.Stop();
        }

        [Test]
        public void returns_500_status_code_if_could_not_get_response_from_replicas()
        {
            var balancer = new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000" }, 1 / 60.0, 1000));

            balancer.Run();

            var exeption = Assert.Throws<WebException>(() => CheckRequest("http://127.0.0.1:20001/method?query=5", "5"));
            Assert.That(exeption.Message.Contains("500"), Is.True);
            balancer.Stop();
        }

        [Test]
        public void trying_get_answer_from_replicas_in_gray_list_if_there_is_no_active()
        {
            var replica = new HashServer(30000);
            var balancer = new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000" }, 3 / 60.0, 1000));
            balancer.Run();
            replica.Start();


            CheckRequest("http://127.0.0.1:20001/method?query=5", "5");
            replica.Stop();
            Assert.Throws<WebException>(() => CheckRequest("http://127.0.0.1:20001/method?query=6", "6"));
            replica.Restart();
            CheckRequest("http://127.0.0.1:20001/method?query=6", "6");
            replica.Stop();
            balancer.Stop();
        }

        [Test]
        public void does_not_accept_requests_without_suffix_method()
        {
            var balancer = new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000" }, 3 / 60.0, 500));
            balancer.Run();

            var exception = Assert.Throws<WebException>(() => CheckRequest("http://127.0.0.1:20001/?query=6", "6"));
            Assert.That(exception.Message.Contains("404"), Is.True);
            balancer.Stop();
        }

        [Test]
        public void returns_compressed_response_if_supported_by_client()
        {
            var replica = new HashServer(30000);
            var balancer = new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000" }, 3 / 60.0, 3000));
            balancer.Run();
            replica.Start();


            CheckRequestSupportCompression("http://127.0.0.1:20001/method?query=5", "5");
            replica.Stop();

            balancer.Stop();
        }

        [Test]
        public void returns_status_code_500_after_timeout()
        {
            var replica = new HashServer(30000, 3000);
            var balancer = new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000" }, 3 / 60.0, 1000));
            balancer.Run();
            replica.Start();

            var exception = Assert.Throws<WebException>(() => CheckRequest("http://127.0.0.1:20001/method?query=6", "6"));
            Assert.That(exception.Message.Contains("500"), Is.True);
            replica.Stop();
            balancer.Stop();

        }

        [Test, Timeout(2000)]
        public void returns_correct_answers_for_multi_threaded_requests()
        {
            var replica = new HashServer(30000);
            var balancer = new Balancer.Balancer(new Settings(20001, new[] { "http://127.0.0.1:30000" }, 3 / 60.0, 3000));
            balancer.Run();
            replica.Start();
            Parallel.For(0, 10, i =>
            {
                GetResponse("http://127.0.0.1:20001/method?query=5");
            });
            replica.Stop();
            balancer.Stop();
        }

        private static void CheckRequest(string request, string expectedResponse)
        {
           Assert.That(GetResponse(request), Is.EqualTo(expectedResponse));
        }

        private static string GetResponse(string request)
        {
            var webRequest = CreateRequest(request, 10000);

            using (var response = webRequest.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    return new StreamReader(responseStream).ReadToEnd();
                }
            }
        }

        private static void CheckRequestSupportCompression(string request, string expectedResponse)
        {
            var webRequest = CreateRequest(request, 10000);
            webRequest.Headers["Accept-Encoding"] = "deflate";

            using (var response = webRequest.GetResponse())
            {
                Assert.That(response.Headers["Content-Encoding"].Contains("deflate"), Is.True);

                using (var responseStream = response.GetResponseStream())
                {
                    var answer = new StreamReader(responseStream).ReadToEnd();
                    var actualDecompessedResponse = Encoding.UTF8.GetString(
                        Encoding.UTF8.GetBytes(answer).DecompressByDeflate());

                    Assert.That(actualDecompessedResponse, Is.EqualTo(expectedResponse));
                }
            }
        }

        private static HttpWebRequest CreateRequest(string uriStr, int timeout)
        {
            var request = WebRequest.CreateHttp(uriStr);
            request.Proxy = null;
            request.Timeout = timeout;
            request.KeepAlive = true;
            request.ServicePoint.UseNagleAlgorithm = false;
            request.ServicePoint.ConnectionLimit = 100;

            return request;
        }
    }
}
