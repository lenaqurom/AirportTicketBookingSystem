using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.Services;
using AirportTicketBookingSystem.UI;
using System;

namespace AirportTicketBookingSystem
{
    class AirportTicketBookingSystemMain
    {
        static void Main(string[] args)
        {
            FlightRepository flightRepository = new FlightRepository();
            FlightService flightService = new FlightService(flightRepository);
            Menu menu = new Menu(flightService);

            menu.ShowMainMenu();
        }
    }
}