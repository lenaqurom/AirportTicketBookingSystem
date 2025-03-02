using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Models
{
    internal class Booking
    {
        public string BookingId { get; set; }
        public string PassengerName { get; set; }
        public string FlightId { get; set; }
        public ClassType Class { get; set; }
        public decimal Price { get; set; }
        public DateTime BookingDate { get; set; }

        public Booking()
        {
            BookingId = Guid.NewGuid().ToString(); 
        }

    }
}
