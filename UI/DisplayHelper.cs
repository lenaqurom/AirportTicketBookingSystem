using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.UI
{
    internal class DisplayHelper
    {
        private readonly ManagerService _managerService;
        private readonly FlightService _flightService;
        private readonly BookingService _bookingService;

        public DisplayHelper(FlightService flightService, BookingService bookingService, ManagerService managerService)
        {
            _flightService = flightService;
            _bookingService = bookingService;
            _managerService = managerService;
        }

        public async Task DisplaySearchFlightsAsync()
        {
            Console.Write("Enter Departure Country (or press Enter to skip): ");
            string departureCountry = Console.ReadLine()?.Trim();

            Console.Write("Enter Destination Country (or press Enter to skip): ");
            string destinationCountry = Console.ReadLine()?.Trim();

            Console.Write("Enter Max Price (or press Enter to skip): ");
            decimal? maxPrice = decimal.TryParse(Console.ReadLine()?.Trim(), out decimal price) ? price : null;

            Console.Write("Enter Departure Date (yyyy-MM-dd) (or press Enter to skip): ");
            DateTime? departureDate = DateTime.TryParse(Console.ReadLine()?.Trim(), out DateTime date) ? date : null;

            Console.Write("Enter Departure Airport (or press Enter to skip): ");
            string departureAirport = Console.ReadLine()?.Trim();

            Console.Write("Enter Arrival Airport (or press Enter to skip): ");
            string arrivalAirport = Console.ReadLine()?.Trim();

            Console.Write("Enter Class (Economy, Business, First) (or press Enter to skip): ");
            string flightClass = Console.ReadLine()?.Trim();

            if (!IsValidFlightClass(flightClass))
            {
                Console.WriteLine("Invalid class. Please enter 'Economy', 'Business', or 'First'.");
                return;
            }

            List<Flight> results = await _flightService.SearchFlightsAsync(
                departureCountry, destinationCountry, maxPrice, departureDate,
                departureAirport, arrivalAirport, flightClass
            );

            flightClass = char.ToUpper(flightClass[0]) + flightClass.Substring(1).ToLower();

            DisplayFlightResults(flightClass, results);
        }

        private static void DisplayFlightResults(string flightClass, List<Flight> results)
        {
            if (results.Any())
            {
                Console.WriteLine("\nMatching Flights:");
                foreach (var flight in results)
                {
                    Console.WriteLine($"Flight ID: {flight.FlightId}, From {flight.DepartureCountry} to {flight.DestinationCountry}, " +
                                      $"Price: {flight.TicketPrices[flightClass]}, Date: {flight.DepartureDate}, Class: {flightClass}");
                }
            }
            else
            {
                Console.WriteLine("No matching flights found.");
            }
        }

        private bool IsValidFlightClass(string flightClass)
        {
            return string.IsNullOrEmpty(flightClass) ||
                   flightClass.Equals("Economy", StringComparison.OrdinalIgnoreCase) ||
                   flightClass.Equals("Business", StringComparison.OrdinalIgnoreCase) ||
                   flightClass.Equals("First", StringComparison.OrdinalIgnoreCase);
        }

        public async Task DisplayAllFlightsAsync()
        {
            List<Flight> flights = await _flightService.ViewFlightsAsync();

            if (flights.Any())
            {
                Console.WriteLine("\nMatching Flights:");
                foreach (var flight in flights)
                {
                    Console.WriteLine($"Flight ID: {flight.FlightId}, From {flight.DepartureCountry} to {flight.DestinationCountry}, Date: {flight.DepartureDate}");
                    foreach (var ticketPrice in flight.TicketPrices)
                    {
                        Console.WriteLine($"Class: {ticketPrice.Key}, Price: {ticketPrice.Value:C}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No matching flights found.");
            }

        }

        public async Task DisplayBookFlightAsync(BookingService bookingSevice, FlightService flightService)
        {
            Console.WriteLine("✈️  Booking a Flight...");
            Console.Write("Enter Flight ID: ");
            string flightId = Console.ReadLine()?.Trim();

            Console.Write("Enter Class (Economy, Business, First): ");
            string flightClass = Console.ReadLine()?.Trim();

            Console.Write("Enter your name: ");
            string passengerName = Console.ReadLine()?.Trim();

            await bookingSevice.BookFlightAsync(flightId, flightClass, passengerName);
        }

        public async Task DisplayAllBookingsAsync()
        {
            List<Booking> bookings = await BookingRepository.GetAllBookingsAsync();
            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
            }
            else
            {
                Console.WriteLine("Bookings:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Booking ID: {booking.BookingId}");
                    Console.WriteLine($"Flight ID: {booking.FlightId}");
                    Console.WriteLine($"Passenger: {booking.Passenger.Name}");
                    Console.WriteLine($"Class: {booking.Class}");
                    Console.WriteLine($"Price: {booking.Price:C}");
                    Console.WriteLine($"Booking Date: {booking.BookingDate}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
    }
}
