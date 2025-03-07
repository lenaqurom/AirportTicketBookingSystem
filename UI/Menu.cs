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
    internal class Menu
    {
        private readonly FlightService _flightService;
        private readonly BookingService _bookingService;
        private readonly ManagerService _managerService;
        public Menu()
        {
            _flightService = new FlightService();
            _bookingService = new BookingService();
            _managerService = new ManagerService();
        }
        public async Task ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Airport Ticket Booking System ===");
                Console.WriteLine("1. Passenger Menu");
                Console.WriteLine("2. Manager Menu");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        await ShowPassengerMenuAsync();
                        break;
                    case "2":
                        await ShowManagerMenuAsync();
                        break;
                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Please choose a valid option.");
                        break;
                }
            }
        }
        private async Task ShowPassengerMenuAsync()
        {
            while (true)
            {
                Console.WriteLine("\n=== Passenger Menu ===");
                Console.WriteLine("1. Search Flights");
                Console.WriteLine("2. View Flights");
                Console.WriteLine("3. Book Flight");
                Console.WriteLine("4. View Bookings");
                Console.WriteLine("5. View Personal Bookings");
                Console.WriteLine("6. Cancel a Booking");
                Console.WriteLine("7. Modify a Booking");
                Console.WriteLine("8. Back to Main Menu");
                Console.Write("Select an option: ");
                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        await SearchFlightsAsync();
                        break;
                    case "2":
                        await ViewAllFlightsAsync();
                        break;
                    case "3":
                        await BookFlightAsync(_bookingService, _flightService);
                        break;
                    case "4":
                        await ViewBookingsAsync();
                        break;
                    case "5":
                        await _bookingService.ViewPersonalBookingsAsync();
                        break;
                    case "6":
                        await _bookingService.CancelBookingAsync();
                        break;
                    case "7":
                        await _bookingService.ModifyBookingAsync();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Please choose a valid option.");
                        break;
                }
            }
        }

        private async Task ShowManagerMenuAsync()
        {
            while (true)
            {
                Console.WriteLine("\n=== Manager Menu ===");
                Console.WriteLine("1. View All Bookings");
                Console.WriteLine("2. Filter Bookings");
                Console.WriteLine("3. Batch Flight Upload (CSV Import)");
                Console.WriteLine("4. Dynamic Model Validation Details");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Select an option: ");
                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        await ViewBookingsAsync();
                        break;
                    case "2":
                        await _managerService.DisplayFilteredBookingsAsync();
                        break;
                    case "3":
                        await _managerService.ImportFlightsAsync();
                        break;
                    case "4":
                        _managerService.DynamicModelValidationDetails();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Please choose a valid option.");
                        break;
                }
            }
        }
        private async Task SearchFlightsAsync()
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

            List<Flight> results = await _flightService.SearchFlights(departureCountry, destinationCountry, maxPrice, departureDate, departureAirport, arrivalAirport, flightClass);

            if (string.IsNullOrEmpty(flightClass) ||
    !(flightClass.Equals("Economy", StringComparison.OrdinalIgnoreCase) ||
      flightClass.Equals("Business", StringComparison.OrdinalIgnoreCase) ||
      flightClass.Equals("First", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Invalid class. Please enter 'Economy', 'Business', or 'First'.");
                return;
            }

            flightClass = char.ToUpper(flightClass[0]) + flightClass.Substring(1).ToLower();

            if (results.Any())
            {
                Console.WriteLine("\nMatching Flights:");
                foreach (var flight in results)
                {
                    Console.WriteLine($"Flight ID: {flight.FlightId}, From {flight.DepartureCountry} to {flight.DestinationCountry}, Price: {flight.TicketPrices[flightClass]}, Date: {flight.DepartureDate}, Class: {flightClass}");
                }
            }
            else
            {
                Console.WriteLine("No matching flights found.");
            }
        }
        private async Task ViewAllFlightsAsync()
        {
            List<Flight> flights = await _flightService.ViewFlightsAsync();
            if (flights.Any())
            {
                Console.WriteLine("\nMatching Flights:");
                foreach (var flight in flights)
                {
                    Console.WriteLine($"Flight ID: {flight.FlightId}, From {flight.DepartureCountry} to {flight.DestinationCountry}, Price: {flight.TicketPrices}, Date: {flight.DepartureDate}, Class: ");
                }
            }
            else
            {
                Console.WriteLine("No matching flights found.");
            }

        }
        private async Task BookFlightAsync(BookingService bookingSevice, FlightService flightService)
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
        private async Task ViewBookingsAsync()
        {
            List<Booking> bookings = await BookingRepository.GetAllBookingsAsync();
            if(bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
            }
            else
            {
                Console.WriteLine("Bookings:");
                foreach(var booking in bookings)
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
