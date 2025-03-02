using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repository;

namespace AirportTicketBookingSystem.Services
{
    internal class FlightService
    {
        private readonly FlightRepository _flightRepository;
        public FlightService()
        {
            _flightRepository = new FlightRepository();
        }
        public List<Flight> SearchFlights(string? departureCountry, string? destinationCountry, decimal? maxPrice, DateTime? departureDate, string? departureAirport, string? arrivalAirport, string? flightClass)
        {
            var flights = _flightRepository.GetAllFlights();

            var filteredFlights = flights.Where(f =>
                (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
                (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date) &&
                (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(flightClass) || (f.TicketPrices.ContainsKey(flightClass) && (!maxPrice.HasValue || f.TicketPrices[flightClass] <= maxPrice.Value)))
            ).ToList();

            return filteredFlights;
        }
    }
}
