using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Balancer
{
    public class Settings
    {
        public int Port { get; private set; }
        public string[] ServersReplicas { get; private set; }
        public double ResidenceTimeInGrayList { get; private set; }
        public int ReplicaAnswerTimeout { get; private set; }

        public Settings(string settingsFilename)
        {
            var lines = File.ReadAllLines(settingsFilename)
                            .Where(line => !string.IsNullOrWhiteSpace(line))
                            .Select(line => line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                            .ToList();

            Port = GetPort(lines);
            ServersReplicas = GetServersReplicas(lines);
            ResidenceTimeInGrayList = GetResidenceTimeInGrayList(lines);
            ReplicaAnswerTimeout = GetReplicaAnswerTimeout(lines);
        }

        public Settings(int port, string[] replicas, double residenceTimeInGrayList, int replicaAnswerTimeout)
        {
            Port = port;
            ServersReplicas = replicas;
            ResidenceTimeInGrayList = residenceTimeInGrayList;
            ReplicaAnswerTimeout = replicaAnswerTimeout;
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

        private static double GetResidenceTimeInGrayList(IEnumerable<string[]> lines)
        {
            var rezidenceTimeLine = lines.FirstOrDefault(line => line[0].Equals("ResidenceTime", StringComparison.InvariantCultureIgnoreCase));
            return double.Parse(rezidenceTimeLine[1]);
        }

        private static int GetReplicaAnswerTimeout(IEnumerable<string[]> lines)
        {
            var rezidenceTimeLine = lines.FirstOrDefault(line => line[0].Equals("ReplicaAnswerTimeout", StringComparison.InvariantCultureIgnoreCase));
            return int.Parse(rezidenceTimeLine[1]);
        }
    }
}
