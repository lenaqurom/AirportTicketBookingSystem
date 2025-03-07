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
    internal class ManagerMenu
    {
        private readonly IManagerService _managerService;
        private readonly DisplayHelper _displayHelper;

        public ManagerMenu(IManagerService managerService, DisplayHelper displayHelper)
        {
            _managerService = managerService;
            _displayHelper = displayHelper;
        }

        public async Task ShowManagerMenuAsync()
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
                        await _displayHelper.DisplayAllBookingsAsync();
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
    }
}
