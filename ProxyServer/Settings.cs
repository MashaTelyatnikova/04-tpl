using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProxyServer
{
    public class Settings
    {
        public int Port { get; private set; }
        public string[] ServersReplicas { get; private set; }

        public Settings(string settingsFilename)
        {
            var lines = File.ReadAllLines(settingsFilename)
                            .Where(line => !string.IsNullOrWhiteSpace(line))
                            .Select(line => line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                            .ToList();

            Port = GetPort(lines);
            ServersReplicas = GetServersReplicas(lines);
        }

        private static int GetPort(IEnumerable<string[]> lines)
        {
            var portLine = lines.FirstOrDefault(line => line[0].Equals("Port", StringComparison.InvariantCultureIgnoreCase));
            return int.Parse(portLine[1]);
        }

        private static string[] GetServersReplicas(IEnumerable<string[]> lines)
        {
            var replicasLine = lines.FirstOrDefault(line => line[0].Equals("ServersReplicas", StringComparison.InvariantCultureIgnoreCase));
            return replicasLine[1].Split(',').ToArray();
        }
    }
}
