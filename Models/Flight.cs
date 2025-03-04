using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Models
{
    internal class Flight
    {
        public string FlightId { get; set; }
        public string DepartureCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureDate { get; set; }
        public Dictionary<string, decimal> TicketPrices { get; set; } = new Dictionary<string, decimal>();

        public static Dictionary<string, List<string>> GetValidationConstraints()
        {
            return new Dictionary<string, List<string>>()
            {
                { "FlightId", new List<string> { "Type: String", "Constraint: Required" } },
                { "DepartureCountry", new List<string> { "Type: Free Text", "Constraint: Required" } },
                { "DestinationCountry", new List<string> { "Type: Free Text", "Constraint: Required" } },
                { "DepartureAirport", new List<string> { "Type: Free Text", "Constraint: Required" } },
                { "ArrivalAirport", new List<string> { "Type: Free Text", "Constraint: Required" } },
                { "DepartureDate", new List<string> { "Type: DateTime", "Constraint: Required", "Allowed Range: today → future" } },
                { "TicketPrices", new List<string> { "Type: Dictionary<string, decimal>", "Constraint: Required", "Allowed Range: Non-negative values" } }
            };
        }
    }
}
