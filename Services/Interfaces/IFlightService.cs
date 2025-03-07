using AirportTicketBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Services.Interfaces
{
    internal interface IFlightService
    {
        List<string> ValidateFlightData(Flight flight, List<string> existingFlightIds);
        Task<List<Flight>> SearchFlightsAsync(string? departureCountry, string? destinationCountry, decimal? maxPrice,
                                              DateTime? departureDate, string? departureAirport, string? arrivalAirport,
                                              string? flightClass);
        Task<List<Flight>> ViewFlightsAsync();
    }
}
