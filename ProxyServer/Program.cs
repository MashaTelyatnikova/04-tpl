namespace ProxyServer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var settings = new Settings("settings.txt");

            var server = new ProxyServer(settings.Port, settings.ServersReplicas);
            server.Run();
        }
    }
}
