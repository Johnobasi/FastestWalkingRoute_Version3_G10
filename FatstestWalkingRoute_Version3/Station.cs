using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatstestWalkingRoute
{
    public class Station
    {
        public string Name { get; set; }
        public List<Connection> Connections { get; set; }

        public Station(string name)
        {
            Name = name;
            Connections = new List<Connection>();
        }
    }
}
