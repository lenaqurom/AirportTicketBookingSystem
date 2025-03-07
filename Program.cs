using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.Services;
using AirportTicketBookingSystem.UI;
using System;
using System.Runtime.CompilerServices;

namespace AirportTicketBookingSystem
{
    class AirportTicketBookingSystemMain
    {
        static async Task  Main(string[] args)
        {
            Menu menu = new Menu();

            await menu.ShowMainMenu();
        }
    }
}