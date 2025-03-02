using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repository;

namespace AirportTicketBookingSystem.Services
{
    internal class ManagerService
    {
        private readonly BookingRepository _bookingRepository;
        public ManagerService() 
        {
            _bookingRepository = new BookingRepository();
        }
        public List<Booking> FilterBookings(
            string? flightId = null,
            decimal? maxPrice = null,
            string? departureCountry = null,
            string? destinationCountry = null,
            DateTime? departureDate = null,
            string? departualAirport = null,
            string? arrivalAirport = null,
            string? passengerName = null,
            string? flightClass = null)
        {
            List<Booking> bookings = BookingRepository.GetAllBookings();
            
            return bookings.Where(b =>
            (string.IsNullOrEmpty(flightId) || b.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(passengerName) || b.PassengerName.Equals(passengerName, StringComparison.OrdinalIgnoreCase)) &&
            (maxPrice == null || b.Price <= maxPrice) &&
            (string.IsNullOrEmpty(flightClass) || b.Class.ToString().Equals(flightClass, StringComparison.OrdinalIgnoreCase))
                ).ToList();
        }
        public void DisplayFilteredBookings()
        {
            Console.Write("Enter Flight ID (or press Enter to skip): ");
            string? flightId = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(flightId)) flightId = null;

            Console.Write("Enter Passenger Name (or press Enter to skip): ");
            string? passengerName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(passengerName)) passengerName = null;

            Console.Write("Enter Maximum Price (or press Enter to skip): ");
            string? maxPriceInput = Console.ReadLine()?.Trim();
            decimal? maxPrice = decimal.TryParse(maxPriceInput, out decimal parcedMaxPrice) ? parcedMaxPrice : null;

            Console.Write("Enter Class (Economy, Business, First) (or press Enter to skip): ");
            string? flightClass = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(flightClass)) flightClass = null;

            List<Booking> filteredBookings = FilterBookings(flightId, maxPrice, null, null, null, null, null, passengerName, flightClass);

            if(filteredBookings.Count == 0)
            {
                Console.WriteLine("No matching bookings found.");
            }
            else
            {
                Console.WriteLine("\nFiltered Bookings:");
                foreach (var booking in filteredBookings)
                {
                    Console.WriteLine($"- Flight: {booking.FlightId}, Passenger: {booking.PassengerName}, Class: {booking.Class}, Price: {booking.Price:C}");
                }
            }
        }
        public void ImportFlights()
        {
            Console.Write("Enter the path to the CSV file: ");
            string csvFilePath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(csvFilePath))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }
            FlightRepository flightRepository = new FlightRepository();
            bool success = flightRepository.ImportFlightsFromCSV(csvFilePath);

            if (success)
            {
                Console.WriteLine("Flight data imported successfully.");
            }
            else
            {
                Console.WriteLine("Failed to import flight data.");
            }
        }
    }
}
