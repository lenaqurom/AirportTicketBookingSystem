using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Models
{
    internal class Booking
    {
        public string BookingId { get; set; }
        public Passenger Passenger { get; set; }
        public string FlightId { get; set; }
        public ClassType Class { get; set; }
        public decimal Price { get; set; }
        public DateTime BookingDate { get; set; }
        public Booking(string passengerName)
        {
            BookingId = Guid.NewGuid().ToString();
            Passenger = new Passenger(passengerName);
        }
        public Booking(Passenger passenger, string flightId, ClassType classType, decimal price)
        {
            BookingId = Guid.NewGuid().ToString();
            Passenger = passenger;
            FlightId = flightId;
            Class = classType;
            Price = price;
            BookingDate = DateTime.Now;
        }
        [JsonConstructor]
        public Booking(string bookingId, Passenger passenger, string flightId,
                       ClassType @class, decimal price, DateTime bookingDate)
        {
            BookingId = bookingId;
            Passenger = passenger ?? throw new ArgumentNullException(nameof(passenger));
            FlightId = flightId;
            Class = @class;
            Price = price;
            BookingDate = bookingDate;
        }
    }
}
