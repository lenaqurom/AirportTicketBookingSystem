using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Repository
{
    internal class FlightRepository
    {
        private readonly string _filePath = "C:\\Users\\ZBOOK\\source\\repos\\AirportTicketBookingSystem\\Data\\flights.json";

        public List<Flight> GetAllFlights()
        {
            if (!File.Exists(_filePath))
                return new List<Flight>();
            string jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Flight>>(jsonData) ?? new List<Flight>();
        } 
        public void SaveFlights(List<Flight> flights)
        {
            string jsonData = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}
