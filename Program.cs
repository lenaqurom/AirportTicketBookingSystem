using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.Services;
using AirportTicketBookingSystem.Services.Interfaces;
using AirportTicketBookingSystem.UI;
using System;
using System.Runtime.CompilerServices;

namespace AirportTicketBookingSystem
{
    class AirportTicketBookingSystemMain
    {
        static async Task Main(string[] args)
        {
            IFlightRepository flightRepository = new FlightRepository();
            IBookingRepository bookingRepository = new BookingRepository();

            IFlightService flightService = new FlightService(flightRepository);
            IBookingService bookingService = new BookingService(flightRepository, bookingRepository);
            IManagerService managerService = new ManagerService(flightRepository);

            MainMenu menu = new MainMenu(flightService, bookingService, managerService);

            await menu.ShowMainMenu();
        }
    }
}
