using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.Services;
using AirportTicketBookingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.UI
{
    internal class PassengerMenu
    {
        private readonly IFlightService _flightService;
        private readonly IBookingService _bookingService;
        private readonly DisplayHelper _displayHelper;

        public PassengerMenu(IFlightService flightService, IBookingService bookingService, DisplayHelper displayHelper)
        {
            _flightService = flightService;
            _bookingService = bookingService;
            _displayHelper = displayHelper;
        }

        public async Task ShowPassengerMenuAsync()
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
                        await _displayHelper.DisplaySearchFlightsAsync();
                        break;
                    case "2":
                        await _displayHelper.DisplayAllFlightsAsync();
                        break;
                    case "3":
                        await _displayHelper.DisplayBookFlightAsync(_bookingService, _flightService);
                        break;
                    case "4":
                        await _displayHelper.DisplayAllBookingsAsync();
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
    }
}
