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
                var options = new JsonSerializerOptions
                {
                    Converters = { new ClassTypeConverter() }
                };
                return JsonSerializer.Deserialize<List<Booking>>(jsonData, options) ?? new List<Booking>();
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
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new ClassTypeConverter() }
                };
                string jsonData = JsonSerializer.Serialize(bookings, options);
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
        public bool CancelBooking(string passengerName, string flightId)
        {
            List<Booking> bookings = GetAllBookings();
            Booking? bookingToRemove = bookings.FirstOrDefault(
                b => b.PassengerName.Equals(passengerName, StringComparison.OrdinalIgnoreCase) &&
                b.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase)
                );
            if (bookingToRemove == null)
            {
                return false;
            }
            bookings.Remove(bookingToRemove);
            SaveBookings(bookings);
            return true;
        }
        public bool ModifyBooking(string passengerName, string flightId, string newClass)
        {
            List<Booking> bookings = GetAllBookings();
            Booking? bookingToModify = bookings.FirstOrDefault(
                b => b.PassengerName.Equals(passengerName,StringComparison.OrdinalIgnoreCase) &&
                b.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase)
                );
            if(bookingToModify == null)
            {
                return false;
            }
            if(Enum.TryParse(newClass, true, out ClassType updatedClass))
            {
                bookingToModify.Class = updatedClass;
                bookingToModify.Price = GetUpdatedPrice(flightId, updatedClass);
            }
            else
            {
                return false;
            }
            SaveBookings(bookings);
            return true;
        }
        public decimal GetUpdatedPrice(string flightId, ClassType classType)
        {
            FlightRepository flightRepo = new FlightRepository();
            Flight? flight = flightRepo.GetFlightById(flightId);
            if(flight == null) { return 0; }
            return classType switch
            {
                ClassType.Economy => flight.TicketPrices["Economy"],
                ClassType.Business => flight.TicketPrices["Business"],
                ClassType.First => flight.TicketPrices["First"],
                _ => 0
            };
        }
    }
}
