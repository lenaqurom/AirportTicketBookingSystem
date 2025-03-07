using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repository;
using AirportTicketBookingSystem.Services.Interfaces;

namespace AirportTicketBookingSystem.Services
{
    internal class FlightService : IFlightService
    {
        private readonly FlightRepository _flightRepository;
        public FlightService()
        {
            _flightRepository = new FlightRepository();
        }
        public List<string> ValidateFlightData(Flight flight, List<string> existingFlightIds)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(flight.FlightId))
                errors.Add("Flight ID is required.");
            else if(existingFlightIds.Contains(flight.FlightId))
                errors.Add($"Duplicate Flight ID detected: {flight.FlightId}");

            if(string.IsNullOrWhiteSpace(flight.DepartureCountry))
                errors.Add("Departure Country is required.");
            if (string.IsNullOrWhiteSpace(flight.DestinationCountry))
                errors.Add("Destination Country is required.");

            if (string.IsNullOrWhiteSpace(flight.DepartureAirport) || flight.DepartureAirport.Length != 3)
                errors.Add("Departure Airport must be a 3-letter code.");
            if (string.IsNullOrWhiteSpace(flight.ArrivalAirport) || flight.ArrivalAirport.Length != 3)
                errors.Add("Arrival Airport must be a 3-letter code.");

            if (flight.DepartureDate < DateTime.UtcNow.Date)
                errors.Add($"Departure date {flight.DepartureDate.ToShortDateString()} is invalid. It must be today or a future date.");

            foreach (var kvp in flight.TicketPrices)
            {
                if (kvp.Value <= 0)
                    errors.Add($"Invalid price for {kvp.Key} class. Price must be greater than zero.");
            }
            
            return errors;
        }
        public async Task<List<Flight>> SearchFlightsAsync(string? departureCountry, string? destinationCountry, decimal? maxPrice, DateTime? departureDate, string? departureAirport, string? arrivalAirport, string? flightClass)
        {
            var flights = await _flightRepository.GetAllFlightsAsync();

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
        public async Task<List<Flight>> ViewFlightsAsync()
        {
            return await _flightRepository.GetAllFlightsAsync();
        }
    }
}
