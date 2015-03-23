using System;
using System.Collections.Concurrent;

namespace Balancer
{
    public class GrayList
    {
        private readonly ConcurrentDictionary<string, DateTime> elements;
        private readonly int residenceTimeInsideInMinutes;

        public GrayList(int residenceTimeInsideInMinutes)
        {
            elements = new ConcurrentDictionary<string, DateTime>();
            this.residenceTimeInsideInMinutes = residenceTimeInsideInMinutes;
        }

        public void AddRecord(string record)
        {
            elements[record] = DateTime.Now;
        }

        public void RemoveRecord(string record)
        {
            DateTime time;
            elements.TryRemove(record, out time);
        }

        public bool ContainsRecord(string record)
        {
            if (!elements.ContainsKey(record))
            {
                return false;
            }

            var lastUpload = elements[record];
            var currentTime = DateTime.Now;

            var elapsed = currentTime.Subtract(lastUpload);
            if (elapsed.TotalSeconds > residenceTimeInsideInMinutes * 60)
            {
                RemoveRecord(record);
                return false;
            }
            return true;
        }
    }
}
