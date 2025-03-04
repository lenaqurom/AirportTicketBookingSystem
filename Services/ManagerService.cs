using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repository;

namespace AirportTicketBookingSystem.Services
{
    internal class ManagerService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly FlightRepository _flightRepository;
        public ManagerService() 
        {
            _bookingRepository = new BookingRepository();
            _flightRepository = new FlightRepository();
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
            List<Flight> flights = _flightRepository.GetAllFlights();

            var filteredBooking = from booking in bookings
                                  join flight in flights on booking.FlightId equals flight.FlightId
                                  where (string.IsNullOrEmpty(flightId) || booking.FlightId.Equals(flightId, StringComparison.OrdinalIgnoreCase)) &&
                                  (string.IsNullOrEmpty(passengerName) || booking.PassengerName.Equals(passengerName, StringComparison.OrdinalIgnoreCase)) &&
                                  (maxPrice == null || booking.Price <= maxPrice) &&
                                  (string.IsNullOrEmpty(flightClass) || booking.Class.ToString().Equals(flightClass, StringComparison.OrdinalIgnoreCase)) &&
                                  (string.IsNullOrEmpty(departureCountry) || flight.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                                  (string.IsNullOrEmpty(destinationCountry) || flight.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                                  (string.IsNullOrEmpty(departualAirport) || flight.DepartureAirport.Equals(departualAirport, StringComparison.OrdinalIgnoreCase)) &&
                                  (string.IsNullOrEmpty(arrivalAirport) || flight.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                                  (departureDate == null || flight.DepartureDate.Date == departureDate.Value.Date)
                                  select booking;

            return filteredBooking.Distinct().ToList();
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
            decimal? maxPrice = decimal.TryParse(maxPriceInput, out decimal parsedMaxPrice) ? parsedMaxPrice : null;

            Console.Write("Enter Departure Country (or press Enter to skip): ");
            string? departureCountry = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(departureCountry)) departureCountry = null;

            Console.Write("Enter Destination Country (or press Enter to skip): ");
            string? destinationCountry = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(destinationCountry)) destinationCountry = null;

            Console.Write("Enter Departure Date (yyyy-MM-dd) (or press Enter to skip): ");
            DateTime? departureDate = DateTime.TryParse(Console.ReadLine()?.Trim(), out DateTime parsedDate) ? parsedDate : null;

            Console.Write("Enter Departure Airport (or press Enter to skip): ");
            string? departureAirport = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(departureAirport)) departureAirport = null;

            Console.Write("Enter Arrival Airport (or press Enter to skip): ");
            string? arrivalAirport = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(arrivalAirport)) arrivalAirport = null;

            Console.Write("Enter Class (Economy, Business, First) (or press Enter to skip): ");
            string? flightClass = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(flightClass)) flightClass = null;

            List<Booking> filteredBookings = FilterBookings(flightId, maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, passengerName, flightClass);

            if (filteredBookings.Count == 0)
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
        public void DynamicModelValidationDetails()
        {
            var validationDetails = Flight.GetValidationConstraints();

            foreach (var field in validationDetails)
            {
                Console.WriteLine($"{field.Key}:");
                foreach (var constraint in field.Value)
                {
                    Console.WriteLine($"  - {constraint}");
                }
            }
        }
    }
}
