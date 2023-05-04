using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatstestWalkingRoute
{
    class TfLUI
    {

        public static TfLApp tflApp;
        public static TfLManager tflManager;
        public TfLUI()
        {
            tflApp = new TfLApp();
            tflManager = new TfLManager();
        }
        public void Start(Dictionary<string, Station> stations)
        {
            ShowMenu(stations); // Display the main menu
        }
        public Dictionary<string, Station> LoadData(string filePath)
        {
            var stations = new Dictionary<string, Station>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || line.Contains("\t")) continue;
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string tubeLine = fileNameWithoutExtension;
                    // Parsing line1
                    string[] line1Parts = line.Split(',');
                    string station1Name = line1Parts[0].Trim();

                    string[] stationB2Parts = line1Parts[1].Trim().Split(' ');
                    string station2Name = string.Join(" ", stationB2Parts.Take(stationB2Parts.Length - 1)).Trim();

                    int time = int.Parse(stationB2Parts[stationB2Parts.Length - 1].Replace("mins", "").Trim());


                    if (!stations.ContainsKey(station1Name))
                    {
                        stations[station1Name] = new Station(station1Name);
                    }
                    if (!stations.ContainsKey(station2Name))
                    {
                        stations[station2Name] = new Station(station2Name);
                    }

                    stations[station1Name].Connections.Add(new Connection(stations[station2Name], time, tubeLine));
                    stations[station2Name].Connections.Add(new Connection(stations[station1Name], time, tubeLine));
                }
            }

            return stations;
        }
        public void ShowMenu(Dictionary<string, Station> stations)
        {
            // Display options to the user
            // Prompt for input and call the appropriate methods based on user's selection
            // Prompt for user type (customer or manager)
            Console.WriteLine("Welcome to Tfl Transport Console App");
            Console.WriteLine("Are you a customer or a manager?");
            Console.WriteLine("1. Customer");
            Console.WriteLine("2. Manager");
            Console.WriteLine("Enter your choice:");

            string userTypeChoice = Console.ReadLine();
            string[] managerOptions = { "1", "2", "3", "5", "6", "7" }; // Manager options
            string[] customerOptions = { "4", "7" }; // Customer options

            bool isManager = userTypeChoice == "2";
            while (true)
                {
                Console.Clear();

                // Display options based on user type
                if (isManager)
                {
                    Console.WriteLine("1. Add walking time delay for a station");
                    Console.WriteLine("2. Remove walking time delay for a station");
                    Console.WriteLine("3. Mark route as impossible for an interchange");
                    Console.WriteLine("5. Find list of delayed walking routes");
                    Console.WriteLine("6. Find list of closed walking routes");
                    Console.WriteLine("7. Exit");
                }
                else
                {
                    Console.WriteLine("4. Find fastest route between two stations");
                    Console.WriteLine("7. Exit");
                }

                Console.WriteLine("Enter your choice:");
                string choice = Console.ReadLine();

                // Check if user choice is valid based on user type
                if ((isManager && !managerOptions.Contains(choice)) || (!isManager && !customerOptions.Contains(choice)))
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue;
                }
                    switch (choice)
                    {
                        case "1":  
                            Station stationA = GetStationNameFromUser(stations,"1st");
                            if (stationA == null)
                            {
                                break;
                            }

                            Station stationB = GetStationNameFromUser(stations,"2nd");
                            if (stationB == null)
                            {
                                break;
                            }

                            Console.WriteLine("Enter delay in minutes:");
                            int delayToAdd = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter Reason of Delay");
                            string reason = Console.ReadLine();

                            tflManager.UpdateWalkingTime(stations,stationA.Name,stationB.Name,delayToAdd,reason);
                            Console.WriteLine($"Added {delayToAdd} minutes delay between {stationA.Name} & {stationB.Name}.");
                            Console.WriteLine("Press Enter to Continue");
                            Console.ReadLine();
                            Console.Clear();    
                        break;

                        case "2":
                            Station stationA2 = GetStationNameFromUser(stations, "1st");
                            if (stationA2 == null)
                            {
                                Console.WriteLine("Press Enter to Continue");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            }

                            Station stationB2 = GetStationNameFromUser(stations, "2nd");
                            if (stationB2 == null)
                            {
                                Console.WriteLine("Press Enter to Continue");
                                Console.ReadLine();
                                Console.Clear();
                            break;
                            }

                            Console.WriteLine("Enter delay in minutes:");
                            int delayToRemove = Convert.ToInt32(Console.ReadLine());
                            tflManager.RemoveWalkingTimeDelay(stations,stationA2.Name,stationB2.Name,0,"");
                            Console.WriteLine($"Removed delay between {stationA2.Name} & {stationB2.Name}..");
                            Console.WriteLine("Press Enter to Continue");
                            Console.ReadLine();
                            Console.Clear();
                        break;

                        case "3":
                            Station stationA3 = GetStationNameFromUser(stations, "1st");
                            if (stationA3 == null)
                            {
                                break;
                            }

                            Station stationB3 = GetStationNameFromUser(stations, "2nd");
                            if (stationB3 == null)
                            {
                                break;
                            }
                            Console.WriteLine("Enter Reason of Closure");
                            string reason3 = Console.ReadLine();
                            tflManager.UpdateRouteStatus(stations,stationA3.Name,stationB3.Name,Status.Closed,reason3);
                            Console.WriteLine($"Marked interchange between {stationA3.Name} and {stationB3.Name} as impossible.");
                            Console.WriteLine("Press Enter to Continue");
                            Console.ReadLine();
                            Console.Clear();
                        break;

                        case "4":
                            Station startStation = GetStationNameFromUser(stations, "1st");
                            if (startStation == null)
                            {
                                Console.WriteLine("Press Enter to Continue");
                                Console.ReadLine();
                                Console.Clear();
                            break;
                            }

                            Station endStation = GetStationNameFromUser(stations, "2nd");
                            if (endStation == null)
                            {
                                Console.WriteLine("Press Enter to Continue");
                                Console.ReadLine();
                                Console.Clear();
                            break;
                            }
                            DisplayShortestPath(tflApp.FindShortestPath(stations,startStation.Name,endStation.Name),startStation.Name,endStation.Name);
                            Console.WriteLine("Press Enter to Continue");
                            Console.ReadLine();
                            Console.Clear();
                        break;

                        case "5":
                            tflApp.PrintDelayedRoutes(stations);
                            Console.WriteLine("Press Enter to Continue");
                            Console.ReadLine();
                            Console.Clear();
                        break;

                        case "6":
                            tflApp.PrintClosedRoutes(stations);
                            Console.WriteLine("Press Enter to Continue");
                            Console.ReadLine();
                            Console.Clear();
                        break;

                        case "7":
                            Console.WriteLine("Exiting...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                        break;
                    }
                }
        }
        public static Station GetStationNameFromUser(Dictionary<string, Station> stations, string arguments)
        {
            Console.WriteLine($"Enter {arguments} station name:");
            string station1Name = Console.ReadLine();
            Station stationA = tflApp.GetStationByName(stationName: station1Name, stations);
            if (stationA == null)
            {
                Console.WriteLine("Station not found. Please try again.");          
            }
            return stationA;
        } 
        public static void DisplayShortestPath(List<Station> path,string startStation,string endStation)
        {
            Console.WriteLine($"Shortest path from {startStation} to {endStation}:");
            Console.WriteLine($"(1) Start: {startStation}");
            int totalTime = 0;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Station station1 = path[i];
                Station station2 = path[i + 1];
                Connection connection = station1.Connections.FirstOrDefault(c => c.Station == station2);
                if (connection != null)
                {
                    Console.WriteLine($"({i + 2}) {station1.Name} ({connection.TubeLine}) to {station2.Name} ({connection.TubeLine}) {connection.Time} min");
                    totalTime += connection.Time;
                }
                else
                {
                    Console.WriteLine($"({i + 2}) Change: {station1.Name} to {station2.Name}");
                }
            }
            Console.WriteLine($"({path.Count + 2}) End: {endStation}");
            Console.WriteLine($"Total Journey Time: {totalTime} minutes");
        }
    }
}
