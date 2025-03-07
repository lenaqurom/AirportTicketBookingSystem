using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Services.Interfaces
{
    internal interface IBookingService
    {
        Task BookFlightAsync(string flightId, string selectedClass, string passengerName);
        Task ViewPersonalBookingsAsync();
        Task CancelBookingAsync();
        Task ModifyBookingAsync();
    }
}
