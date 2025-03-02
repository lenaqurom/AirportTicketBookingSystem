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
            Menu menu = new Menu();

            menu.ShowMainMenu();
        }
    }
}