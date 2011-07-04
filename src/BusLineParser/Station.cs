using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzBusSystem.BusLineParser
{
    public class Station
    {
        public string StationName { get; set; }

        public List<string> LinesPast { get; set; }
    }
}
