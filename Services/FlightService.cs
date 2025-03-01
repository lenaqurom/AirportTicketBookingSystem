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
        public FlightService(FlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }
        public List<Flight> SearchFlights(string departure, string destination, DateTime? date, string seatClass)
        {
            var flights = _flightRepository.GetAllFlights();

            return flights.Where(f =>
            f.DepartureCountry.Equals(departure, StringComparison.OrdinalIgnoreCase) &&
            f.DestinationCountry.Equals(destination, StringComparison.OrdinalIgnoreCase) &&
            (!date.HasValue || f.DepartureDate.Date == date.Value.Date) &&
            f.TicketPrices.ContainsKey(seatClass)
            ).ToList();
        }
    }
}
