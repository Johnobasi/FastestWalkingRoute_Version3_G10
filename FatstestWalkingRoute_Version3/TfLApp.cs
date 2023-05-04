using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatstestWalkingRoute
{
    class TfLApp
    {
        public List<Station> FindShortestPath(Dictionary<string, Station> stations, string startStationName, string endStationName)
        {
            var visited = new HashSet<string>();
            var distances = new Dictionary<string, int>();
            var previousStations = new Dictionary<string, string>();

            Station startStation = stations[startStationName];
            Station endStation = stations[endStationName];

            foreach (var station in stations.Values)
            {
                if (station.Name == startStation.Name)
                {
                    distances[station.Name] = 0;
                }
                else
                {
                    distances[station.Name] = int.MaxValue;
                }
                previousStations[station.Name] = null;
            }

            while (visited.Count < stations.Count)
            {
                var currentStation = stations
                    .Where(s => !visited.Contains(s.Key))
                    .OrderBy(s => distances[s.Key])
                    .First().Value;

                if (currentStation.Name == endStation.Name)
                {
                    break;
                }

                visited.Add(currentStation.Name);

                foreach (var connection in currentStation.Connections)
                {
                    if (!visited.Contains(connection.Station.Name))
                    {
                        int alt = distances[currentStation.Name] + connection.Time;
                        if (alt < distances[connection.Station.Name])
                        {
                            distances[connection.Station.Name] = alt;
                            previousStations[connection.Station.Name] = currentStation.Name;
                        }
                    }
                }
            }

            List<Station> path = new List<Station>();
            string current = endStation.Name;
            while (current != null)
            {
                path.Add(stations[current]);
                current = previousStations[current];
            }
            path.Reverse();

            return path;
        }
        public  Station GetStationByName(string stationName, Dictionary<string, Station> stations)
        {
            if (stations.TryGetValue(stationName, out var start))
            {
                return start;
            }
            else
            {
                Console.WriteLine($"Start station '{stationName}' not found.");
                return null;
            }
        }
        public void PrintDelayedRoutes(Dictionary<string, Station> stations)
        {
            Console.WriteLine("Delayed routes:");
            foreach (var station in stations.Values)
            {
                foreach (var connection in station.Connections)
                {
                    if (connection.Status == Status.Delayed)
                    {
                        Console.WriteLine($"{station.Name} - {connection.Station.Name} : {connection.OriginalTime} min now {connection.Time} min ({connection.StatusReason})");
                    }
                }
            }
        }
        public void PrintClosedRoutes(Dictionary<string, Station> stations)
        {
            Console.WriteLine("Closed routes:");
            foreach (var station in stations.Values)
            {
                foreach (var connection in station.Connections)
                {
                    if (connection.Status == Status.Closed)
                    {
                        Console.WriteLine($"{connection.TubeLine}: {station.Name} - {connection.Station.Name} : {connection.StatusReason}");
                    }
                }
            }
        }

    }
}
