using AirportTicketBookingSystem.Models;
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
        public Menu(FlightService flightService)
        {
            _flightService = flightService;
        }
        public void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Search Flights");
                Console.WriteLine("2. Exit");
                Console.Write("Choose an option: ");

                string choise = Console.ReadLine();
                if (choise == "1")
                    SearchFlights();
                else if (choise == "2")
                    break;
            }
        }
        private void SearchFlights()
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

            List<Flight> results = _flightService.SearchFlights(departureCountry, destinationCountry, maxPrice, departureDate, departureAirport, arrivalAirport, flightClass);

            if (results.Any())
            {
                Console.WriteLine("\nMatching Flights:");
                foreach (var flight in results)
                {
                    Console.WriteLine($"Flight ID: {flight.FlightId}, From {flight.DepartureCountry} to {flight.DestinationCountry}, Price: {flight.Price}, Date: {flight.DepartureDate}, Class: {flight.Class}");
                }
            }
            else
            {
                Console.WriteLine("No matching flights found.");
            }
        }
    }
}
