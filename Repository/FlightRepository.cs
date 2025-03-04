using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using AirportTicketBookingSystem.Models;
using System.Globalization;

namespace AirportTicketBookingSystem.Repository
{
    internal class FlightRepository
    {
        private readonly string _filePath = "C:\\Users\\ZBOOK\\source\\repos\\AirportTicketBookingSystem\\Data\\flights.json";

        public List<Flight> GetAllFlights()
        {
            if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
                return new List<Flight>();
            try
            {
                string jsonData = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Flight>>(jsonData) ?? new List<Flight>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading flight data: {ex.Message}");
                return new List<Flight>();
            }
        }
        public void SaveFlights(List<Flight> flights)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving flight data: {ex.Message}");
            }
        }
        public Flight? GetFlightById(string flightId)
        {
            List<Flight> flights = GetAllFlights();
            return flights.FirstOrDefault(f => f.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase));
        }
        public bool ImportFlightsFromCSV(string csvFilePath)
        {
            if(!File.Exists(csvFilePath))
            {
                Console.WriteLine("CSV file not found.");
                return false;
            }
            List<Flight> flights = GetAllFlights();
            List<string> errors = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(csvFilePath);
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] parts = line.Split(",");

                    if (parts.Length < 9)
                    {
                        errors.Add($"Line {i + 1}: Invalid format, expected at least 9 fields.");
                        continue;
                    }
                    string flightId = parts[0].Trim();
                    string departureCountry = parts[1].Trim();
                    string destinationCountry = parts[2].Trim();
                    string departureAirport = parts[3].Trim();
                    string arrivalAirport = parts[4].Trim();

                    if (string.IsNullOrWhiteSpace(flightId) ||
                        string.IsNullOrWhiteSpace(departureCountry) ||
                        string.IsNullOrWhiteSpace(destinationCountry) ||
                        string.IsNullOrWhiteSpace(departureAirport) ||
                        string.IsNullOrWhiteSpace(arrivalAirport))
                    {
                        errors.Add($"Line {i + 1}: One or more required fields are empty.");
                        continue;
                    }

                    if (!DateTime.TryParseExact(parts[5], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDate))
                    {
                        errors.Add($"Line {i + 1}: Invalid departure date format.");
                        continue;
                    }

                    if (!decimal.TryParse(parts[6].Trim(), out decimal economyPrice) || economyPrice < 0 ||
                        !decimal.TryParse(parts[7].Trim(), out decimal businessPrice) || businessPrice < 0 ||
                        !decimal.TryParse(parts[8].Trim(), out decimal firstClassPrice) || firstClassPrice < 0)
                    {
                        errors.Add($"Line {i + 1}: Invalid or negative ticket prices.");
                        continue;
                    }

                    Dictionary<string, decimal> ticketPrices = new Dictionary<string, decimal>
                    {
                        { "Economy", decimal.Parse(parts[6].Trim())},
                        { "Buisness", decimal.Parse(parts[7].Trim())},
                        { "First", decimal.Parse(parts[8].Trim())}
                    };
                    Flight newFlight = new Flight
                    {
                        FlightId = flightId,
                        DepartureCountry = departureCountry,
                        DestinationCountry = destinationCountry,
                        DepartureAirport = departureAirport,
                        ArrivalAirport = arrivalAirport,
                        DepartureDate = departureDate,
                        TicketPrices = ticketPrices
                    };
                    flights.Add(newFlight);
                }

                if (errors.Any())
                {
                    Console.WriteLine("Errors found in CSV file:");
                    errors.ForEach(Console.WriteLine);
                    return false;
                }

                SaveFlights(flights);
                Console.WriteLine("Flights imported successfully.");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error importing flights: {ex.Message}");
                return false;
            }
        }
    }
}
