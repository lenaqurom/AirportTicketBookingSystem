using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Repository
{
    internal class BookingRepository
    {
        private static readonly string BookingsFilePath = "C:\\\\Users\\\\ZBOOK\\\\source\\\\repos\\\\AirportTicketBookingSystem\\\\Data\\\\bookings.json";
        public static List<Booking> GetAllBookings()
        {
            if(!File.Exists(BookingsFilePath) || new FileInfo(BookingsFilePath).Length == 0)
            {
                return new List<Booking>();
            }
            try
            {
                string jsonData = File.ReadAllText(BookingsFilePath);
                return JsonSerializer.Deserialize<List<Booking>>(jsonData) ?? new List<Booking>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading booking data: {ex.Message}");
                return new List<Booking>();
            }
        }
        public void SaveBookings(List<Booking> bookings)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(bookings, new JsonSerializerOptions { WriteIndented = true});
                File.WriteAllText(BookingsFilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving booking data: {ex.Message}");
            }
        }
        public List<Booking> GetBookingsByPassenger(string passengerName) 
        { 
            List<Booking> allBookings = GetAllBookings();
            List<Booking> passengerBookings = allBookings
                .Where(p => p.PassengerName.Equals(passengerName, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return passengerBookings;
        }
    }
}
