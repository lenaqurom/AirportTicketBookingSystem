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
    internal class MainMenu
    {
        private readonly PassengerMenu _passengerMenu;
        private readonly ManagerMenu _managerMenu;
        private readonly DisplayHelper _displayHelper;

        private readonly FlightService _flightService;
        private readonly BookingService _bookingService;
        private readonly ManagerService _managerService;

        public MainMenu()
        {
            _flightService = new FlightService();
            _bookingService = new BookingService();
            _managerService = new ManagerService();
            _displayHelper = new DisplayHelper(_flightService, _bookingService, _managerService);

            _passengerMenu = new PassengerMenu(_flightService, _bookingService, _displayHelper);
            _managerMenu = new ManagerMenu(_managerService, _displayHelper);
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
                        await _passengerMenu.ShowPassengerMenuAsync();
                        break;
                    case "2":
                        await _managerMenu.ShowManagerMenuAsync();
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
    }
}
