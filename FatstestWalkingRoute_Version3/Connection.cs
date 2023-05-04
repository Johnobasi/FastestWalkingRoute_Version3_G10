using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatstestWalkingRoute
{
    public class Connection
    {
        public Station Station { get; set; }
        public int Time { get; set; }
        public string TubeLine { get; set; }

        public int OriginalTime { get; set; }
        public Status Status { get; set; }
        public string StatusReason { get; set; }

        public Connection(Station station, int time,string tubeLine)
        {
            Station = station;
            Time = time;
            TubeLine = tubeLine;
            OriginalTime = time;
            Status = Status.Open;
            StatusReason = "";
        }
    }

    public enum Status
    {
        Open,
        Closed,
        Delayed
    }


}
