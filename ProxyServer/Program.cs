namespace Balancer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var settings = new Settings("settings.txt");

            var server = new Balancer(settings);
            server.Run();
        }
    }
}
