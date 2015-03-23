using System;
using System.Collections.Generic;

namespace Balancer
{
    public class GrayList
    {
        private readonly Dictionary<string, DateTime> elements;
        private readonly int residenceTimeInsideInMinutes;

        public GrayList(int residenceTimeInsideInMinutes)
        {
            elements = new Dictionary<string, DateTime>();
            this.residenceTimeInsideInMinutes = residenceTimeInsideInMinutes;
        }

        public void AddRecord(string record)
        {
            elements[record] = DateTime.Now;
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
                elements.Remove(record);
                return false;
            }
            return true;
        }
    }
}
