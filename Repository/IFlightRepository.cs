using AirportTicketBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Repository
{
    internal interface IFlightRepository
    {
        Task<List<Flight>> GetAllFlightsAsync();
        Task SaveFlightsAsync(List<Flight> flights);
        Task<Flight?> GetFlightByIdAsync(string flightId);
        Task<bool> ImportFlightsFromCSVAsync(string csvFilePath);
    }
}
