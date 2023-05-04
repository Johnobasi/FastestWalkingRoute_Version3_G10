
using FatstestWalkingRoute;
using FatstestWalkingRoute_Version3;
using System.Linq;
// See https://aka.ms/new-console-template for more information
Dictionary<string, Station> stations = new Dictionary<string, Station>();
string filePath0 = "Data/Central.txt";
string filePath1 = "Data/Bakerloo.txt";
string filePath2 = "Data/Circle.txt";
string filePath3 = "Data/District.txt";
string filePath4 = "Data/H&C.txt";
string filePath5 = "Data/Jubilee.txt";
string filePath6 = "Data/Metropolitan.txt";
string filePath7 = "Data/Northern.txt";
string filePath8 = "Data/Piccadilly.txt";
string filePath9 = "Data/Victoria.txt";
string filePath10 = "Data/Waterloo.txt";

string[] filePaths = new string[] {filePath0,filePath1,filePath2,filePath3,filePath4,filePath5,filePath6,filePath7,filePath8,filePath9,filePath10};


TfLUI tfLUI = new TfLUI();
TfLApp tfLApp = new TfLApp();
TfLManager tfLManager = new TfLManager();

// Loop through each file path
foreach (var filePath in filePaths)
{
    // Load data from the current file
    Dictionary<string, Station> stationsFromFile = tfLUI.LoadData(filePath);

    // Merge data from the current file into the main dictionary
    foreach (var station in stationsFromFile)
    {
        // Check if station key already exists in the main dictionary
        if (!stations.ContainsKey(station.Key))
        {
            // If not, add the station to the main dictionary
            stations.Add(station.Key, station.Value);
        }
        else
        {
            // If the station key already exists in the main dictionary, you can choose to update or skip it
            // For example, you can update the station values like this:
            // stations[station.Key] = station.Value;
            // Or you can skip it like this:
            // continue;
            // Merge connections from the current file into the existing station
            foreach (var connection in station.Value.Connections)
            {
                if (!stations[station.Key].Connections.Any(c => c.Station.Name == connection.Station.Name && c.TubeLine == connection.TubeLine))
                {
                    stations[station.Key].Connections.Add(connection);
                }
            }
        }
    }
}


TfLAppTester tester = new TfLAppTester(stations);
while (true)
{
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. Run tests");
    Console.WriteLine("2. Go to the main app");
    Console.WriteLine("3. Exit");

    int option;
    if (int.TryParse(Console.ReadLine(), out option))
    {
        switch (option)
        {
            case 1:
                tester.RunTests(); // Implement this method to run your tests
                break;
            case 2:
                tfLUI.Start(stations); // Implement this method to run your main app
                break;
            case 3:
                Console.WriteLine("Exiting...");
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please try again.");
    }
}


//stations = tfLUI.LoadData(filePath);
//stations.Append(tfLUI.LoadData(filePath1));











