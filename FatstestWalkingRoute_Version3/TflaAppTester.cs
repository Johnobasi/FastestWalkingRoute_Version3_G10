using FatstestWalkingRoute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatstestWalkingRoute_Version3
{
    public class TfLAppTester
    {
        Dictionary<string, Station> stations = new Dictionary<string, Station>();

        public TfLAppTester(Dictionary<string, Station> _stations)
        {
            this.stations = _stations;
        }

        public void RunTests()
        {
            // Move the content of the Main method here
            // ...
            // Initialize stations and connections data
            // ...
            TfLApp tflApp = new TfLApp();
            List<(string, string)> testCases = new List<(string, string)>
            {
                ("Elephant and Castle", "Charing Cross"),
                ("Elephant and Castle", "Marylebone"),
                ("Edgware Road (Circle Line)", "Aldgate East"),
                // Add more test cases as needed
            };

            // Consistency tests
            Console.WriteLine("Consistency Test Results:");
            Console.WriteLine("Start Station | End Station | Fastest Time");
            Console.WriteLine("------------------------------------------");
            foreach (var testCase in testCases)
            {
                // Find the shortest path and calculate the total time
                List<Station> stationsList = tflApp.FindShortestPath(stations, testCase.Item1, testCase.Item2);
                int totalTime = CalculateTotalTime(stationsList, testCase.Item1, testCase.Item2);
                Console.WriteLine($"{testCase.Item1} | {testCase.Item2} | {totalTime}");
            }
            Console.WriteLine();

            // Benchmarking tests
            Console.WriteLine("Benchmarking Test Results:");
            Console.WriteLine("Start Station | End Station | Average Execution Time (ms)");
            Console.WriteLine("-------------------------------------------------------");
            foreach (var testCase in testCases)
            {
                int runs = 10;
                double averageExecutionTime = Benchmark(stations, tflApp, testCase.Item1, testCase.Item2, runs);
                Console.WriteLine($"{testCase.Item1} | {testCase.Item2} | {averageExecutionTime}");
            }
            Console.ReadLine();
            Console.Clear();
        }

        public static int CalculateTotalTime(List<Station> path, string startStation, string endStation)
        {
          //  Console.WriteLine($"Shortest path from {startStation} to {endStation}:");
          //  Console.WriteLine($"(1) Start: {startStation}");
            int totalTime = 0;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Station station1 = path[i];
                Station station2 = path[i + 1];
                Connection connection = station1.Connections.FirstOrDefault(c => c.Station == station2);
                if (connection != null)
                {
              //      Console.WriteLine($"({i + 2}) {station1.Name} ({connection.TubeLine}) to {station2.Name} ({connection.TubeLine}) {connection.Time} min");
                    totalTime += connection.Time;
                }
                else
                {
                //    Console.WriteLine($"({i + 2}) Change: {station1.Name} to {station2.Name}");
                }
            }
            //Console.WriteLine($"({path.Count + 2}) End: {endStation}");
            //Console.WriteLine($"Total Journey Time: {totalTime} minutes");
            return totalTime;
        }
        static double Benchmark(Dictionary<string, Station> stations, TfLApp tflApp, string start, string end, int runs)
        {
            Stopwatch stopwatch = new Stopwatch();
            double totalTime = 0;

            for (int i = 0; i < runs; i++)
            {
                stopwatch.Restart();
                tflApp.FindShortestPath(stations, start, end);
                stopwatch.Stop();
                totalTime += stopwatch.Elapsed.TotalMilliseconds;
            }

            return totalTime / runs;
        }


    }
}
