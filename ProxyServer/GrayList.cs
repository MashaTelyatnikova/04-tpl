using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyServer
{
    public class GrayList
    {
        private readonly Dictionary<string, DateTime> elements;
        private readonly int timeout;
        public GrayList(int timeout)
        {
            elements = new Dictionary<string, DateTime>();
            this.timeout = timeout;
        }

        public void Add(string element)
        {
            elements[element] = DateTime.Now;
        }

        public bool Contais(string element)
        {
            if (!elements.ContainsKey(element))
            {
                return false;
            }

            var date = elements[element];
            var current = DateTime.Now;

            var dif = current.Subtract(date);
            if (dif.TotalSeconds > timeout * 60)
            {
                elements.Remove(element);
                return false;
            }
            return true;
        }
    }
}
