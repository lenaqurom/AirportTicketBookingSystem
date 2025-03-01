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
            Console.Write("Enter Departure Country: ");
            string departure = Console.ReadLine();

            Console.Write("Enter Destination Country: ");
            string destination = Console.ReadLine();

            Console.Write("Enter Date (YYYY-MM-DD) or press Enter to skip: ");
            string dateInput = Console.ReadLine();
            DateTime? date = string.IsNullOrWhiteSpace(dateInput) ? null : DateTime.Parse(dateInput);

            Console.Write("Enter Seat Class (Economy, Business, First): ");
            string seatClass = Console.ReadLine();

            var result = _flightService.SearchFlights(departure, destination, date, seatClass);
            if (result.Count == 0)
            {
                Console.WriteLine("No flights found.");
                return;
            }
            foreach (var flight in result)
            {
                Console.WriteLine($"Flight {flight.FlightId} | {flight.DepartureAirport} → {flight.ArrivalAirport} | {flight.DepartureDate} | Price: {flight.TicketPrices[seatClass]}");
            }
        }
    }
}
