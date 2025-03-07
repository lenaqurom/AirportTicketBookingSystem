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
        public static async Task<List<Booking>> GetAllBookingsAsync()
        {
            if(!File.Exists(BookingsFilePath) || new FileInfo(BookingsFilePath).Length == 0)
            {
                Console.WriteLine($"File does not Exist.");
                return new List<Booking>();
            }
            try
            {
                string jsonData = await File.ReadAllTextAsync(BookingsFilePath);
                var options = new JsonSerializerOptions
                {
                    Converters = { new ClassTypeConverter() },
                    IncludeFields = true
                };
                return JsonSerializer.Deserialize<List<Booking>>(jsonData, options) ?? new List<Booking>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading booking data: {ex.Message}");
                return new List<Booking>();
            }
        }
        public async Task SaveBookingsAsync(List<Booking> bookings)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new ClassTypeConverter() }
                };
                string jsonData = JsonSerializer.Serialize(bookings, options);
                await File.WriteAllTextAsync(BookingsFilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving booking data: {ex.Message}");
            }
        }
        public async Task<List<Booking>> GetBookingsByPassengerAsync(string passengerName) 
        { 
            List<Booking> allBookings = await GetAllBookingsAsync();
            List<Booking> passengerBookings = allBookings
                .Where(p => p.Passenger.Name.Equals(passengerName, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return passengerBookings;
        }
        public async Task<bool> CancelBookingAsync(string passengerName, string flightId)
        {
            List<Booking> bookings = await GetAllBookingsAsync();
            Booking? bookingToRemove = bookings.FirstOrDefault(
                b => b.Passenger.Name.Equals(passengerName, StringComparison.OrdinalIgnoreCase) &&
                b.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase)
                );
            if (bookingToRemove == null)
            {
                return false;
            }
            bookings.Remove(bookingToRemove);
            await SaveBookingsAsync(bookings);
            return true;
        }
        public async Task<bool> ModifyBookingAsync(string passengerName, string flightId, string newClass)
        {
            List<Booking> bookings = await GetAllBookingsAsync();
            Booking? bookingToModify = bookings.FirstOrDefault(
                b => b.Passenger.Name.Equals(passengerName,StringComparison.OrdinalIgnoreCase) &&
                b.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase)
                );
            if(bookingToModify == null)
            {
                return false;
            }
            if(Enum.TryParse(newClass, true, out ClassType updatedClass))
            {
                bookingToModify.Class = updatedClass;
                bookingToModify.Price = await GetUpdatedPriceAsync(flightId, updatedClass);
            }
            else
            {
                return false;
            }
            await SaveBookingsAsync(bookings);
            return true;
        }
        public async Task<decimal> GetUpdatedPriceAsync(string flightId, ClassType classType)
        {
            FlightRepository flightRepo = new FlightRepository();
            Flight? flight = await flightRepo.GetFlightByIdAsync(flightId);
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
