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
    internal class FlightRepository : IFlightRepository
    {
        private readonly string _filePath = "C:\\Users\\ZBOOK\\source\\repos\\AirportTicketBookingSystem\\Data\\flights.json";

        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
                return new List<Flight>();
            try
            {
                string jsonData = await File.ReadAllTextAsync(_filePath);
                List<Flight> flights = JsonSerializer.Deserialize<List<Flight>>(jsonData) ?? new List<Flight>();

                return flights.Where(f => ValidateFlight(f, out List<string> _)).ToList();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading flight data: {ex.Message}");
                return new List<Flight>();
            }
        }
        public async Task SaveFlightsAsync(List<Flight> flights)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllTextAsync(_filePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving flight data: {ex.Message}");
            }
        }
        public async Task<Flight?> GetFlightByIdAsync(string flightId)
        {
            List<Flight> flights = await GetAllFlightsAsync();
            return flights.Where(f => f.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
        public async Task<bool> ImportFlightsFromCSVAsync(string csvFilePath)
        {
            if(!File.Exists(csvFilePath))
            {
                Console.WriteLine("CSV file not found.");
                return false;
            }
            List<Flight> flights = await GetAllFlightsAsync();
            List<string> errors = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(csvFilePath);
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (!ValidateCsvLine(line, i, out List<string> lineErrors, out Flight? newFlight))
                    {
                        errors.AddRange(lineErrors);
                        continue;
                    }
                    if (flights.Any(f => f.FlightId == newFlight?.FlightId))
                    {
                        errors.Add($"Line {i + 1}: Flight ID '{newFlight?.FlightId}' already exists.");
                        continue;
                    }

                    flights.Add(newFlight);
                }

                if (errors.Any())
                {
                    Console.WriteLine("Errors found in CSV file:");
                    errors.ForEach(Console.WriteLine);
                    return false;
                }

                await SaveFlightsAsync(flights);
                Console.WriteLine("Flights imported successfully.");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error importing flights: {ex.Message}");
                return false;
            }
        }

        private bool ValidateFlight(Flight flight, out List<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(flight.FlightId))
                errorMessages.Add("Flight ID is empty.");
            if (string.IsNullOrWhiteSpace(flight.DepartureCountry))
                errorMessages.Add("Departure Country is empty.");
            if (string.IsNullOrWhiteSpace(flight.DestinationCountry))
                errorMessages.Add("Destination Country is empty.");
            if (string.IsNullOrWhiteSpace(flight.DepartureAirport))
                errorMessages.Add("Departure Airport is empty.");
            if (string.IsNullOrWhiteSpace(flight.ArrivalAirport))
                errorMessages.Add("Arrival Airport is empty.");

            if (flight.DepartureDate == default)
                errorMessages.Add("Departure Date is invalid.");

            if (flight.TicketPrices == null || !flight.TicketPrices.Any())
                errorMessages.Add("Ticket prices are not defined or invalid.");

            foreach (var price in flight.TicketPrices.Values)
            {
                if (price <= 0)
                    errorMessages.Add("Ticket price cannot be negative.");
            }

            return !errorMessages.Any();
        }

        private bool ValidateCsvLine(string line, int lineNumber, out List<string> errorMessages, out Flight? flight)
        {
            errorMessages = new List<string>();
            flight = null;

            string[] parts = line.Split(",");

            if (parts.Length < 9)
            {
                errorMessages.Add($"Line {lineNumber + 1}: Invalid format, expected at least 9 fields.");
                return false;
            }
            string flightId = parts[0].Trim();
            string departureCountry = parts[1].Trim();
            string destinationCountry = parts[2].Trim();
            string departureAirport = parts[3].Trim();
            string arrivalAirport = parts[4].Trim();

            if (string.IsNullOrWhiteSpace(flightId))
                errorMessages.Add($"Line {lineNumber + 1}: Flight ID is empty.");
            if (string.IsNullOrWhiteSpace(departureCountry))
                errorMessages.Add($"Line {lineNumber + 1}: Departure Country is empty.");
            if (string.IsNullOrWhiteSpace(destinationCountry))
                errorMessages.Add($"Line {lineNumber + 1}: Destination Country is empty.");
            if (string.IsNullOrWhiteSpace(departureAirport))
                errorMessages.Add($"Line {lineNumber + 1}: Departure Airport is empty.");
            if (string.IsNullOrWhiteSpace(arrivalAirport))
                errorMessages.Add($"Line {lineNumber + 1}: Arrival Airport is empty.");

            if (!DateTime.TryParseExact(parts[5], "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDate))
                errorMessages.Add($"Line {lineNumber + 1}: Invalid departure date format (Expected: yyyy-MM-dd HH:mm).");

            if (!decimal.TryParse(parts[6].Trim(), out decimal economyPrice) || economyPrice < 0)
                errorMessages.Add($"Line {lineNumber + 1}: Invalid economy class ticket price.");
            if (!decimal.TryParse(parts[7].Trim(), out decimal businessPrice) || businessPrice < 0)
                errorMessages.Add($"Line {lineNumber + 1}: Invalid business class ticket price.");
            if (!decimal.TryParse(parts[8].Trim(), out decimal firstClassPrice) || firstClassPrice < 0)
                errorMessages.Add($"Line {lineNumber + 1}: Invalid first class ticket price.");

            if (errorMessages.Any())
                return false;

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

            return true;
        }
    }
}
