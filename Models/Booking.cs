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
        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
        public string SeatClass { get; set; }
        public decimal Price { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
