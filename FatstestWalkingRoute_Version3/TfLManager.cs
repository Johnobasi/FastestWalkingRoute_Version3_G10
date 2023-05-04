using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatstestWalkingRoute
{
    class TfLManager
    {
        public void UpdateWalkingTime(Dictionary<string, Station> stations, string stationA, string stationB, int delay, string reason)
        {
            if (stations.TryGetValue(stationA, out Station a) && stations.TryGetValue(stationB, out Station b))
            {
                var connectionAtoB = a.Connections.FirstOrDefault(c => c.Station.Name == stationB);
                var connectionBtoA = b.Connections.FirstOrDefault(c => c.Station.Name == stationA);

                if (connectionAtoB != null)
                {
                    connectionAtoB.Time += delay;
                    connectionAtoB.Status = Status.Delayed;
                    connectionAtoB.StatusReason = reason;
                }
                if (connectionBtoA != null)
                {
                    connectionBtoA.Time += delay;
                    connectionBtoA.Status = Status.Delayed;
                    connectionBtoA.StatusReason = reason;
                }
            }
            else
            {
                Console.WriteLine("One or both of the specified stations do not exist.");
            }
        }
        public void RemoveWalkingTimeDelay(Dictionary<string, Station> stations, string stationA, string stationB, int delay, string reason)
        {
            if (stations.TryGetValue(stationA, out Station a) && stations.TryGetValue(stationB, out Station b))
            {
                var connectionAtoB = a.Connections.FirstOrDefault(c => c.Station.Name == stationB);
                var connectionBtoA = b.Connections.FirstOrDefault(c => c.Station.Name == stationA);

                if (connectionAtoB != null)
                {
                    connectionAtoB.Time = connectionAtoB.OriginalTime;
                    connectionAtoB.Status = Status.Open;
                    connectionAtoB.StatusReason = reason;
                }
                if (connectionBtoA != null)
                {
                    connectionBtoA.Time = connectionBtoA.OriginalTime;
                    connectionBtoA.Status = Status.Open;
                    connectionBtoA.StatusReason = reason;
                }
            }
            else
            {
                Console.WriteLine("One or both of the specified stations do not exist.");
            }
        }
        public void UpdateRouteStatus(Dictionary<string, Station> stations, string stationA, string stationB, Status status, string reason = null)
        {
            if (stations.TryGetValue(stationA, out Station a) && stations.TryGetValue(stationB, out Station b))
            {
                var connectionAtoB = a.Connections.FirstOrDefault(c => c.Station.Name == stationB);
                var connectionBtoA = b.Connections.FirstOrDefault(c => c.Station.Name == stationA);

                if (connectionAtoB != null)
                {
                    connectionAtoB.Status = status;
                    connectionAtoB.StatusReason = reason;
                }
                if (connectionBtoA != null)
                {
                    connectionBtoA.Status = status;
                    connectionBtoA.StatusReason = reason;
                }
            }
            else
            {
                Console.WriteLine("One or both of the specified stations do not exist.");
            }
        }

    }

}
