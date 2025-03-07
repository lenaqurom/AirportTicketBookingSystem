using AirportTicketBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Services.Interfaces
{
    internal interface IManagerService
    {
        Task<List<Booking>> FilterBookingsAsync(
            string? flightId = null,
            decimal? maxPrice = null,
            string? departureCountry = null,
            string? destinationCountry = null,
            DateTime? departureDate = null,
            string? departureAirport = null,
            string? arrivalAirport = null,
            string? passengerName = null,
            string? flightClass = null);

        Task DisplayFilteredBookingsAsync();
        Task ImportFlightsAsync();
        void DynamicModelValidationDetails();
    }
}
