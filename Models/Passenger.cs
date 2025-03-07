using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AirportTicketBookingSystem.Models
{
    internal class Passenger
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Passenger(string name) 
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }
    }
}
