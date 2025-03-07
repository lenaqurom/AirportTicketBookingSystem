using AirportTicketBookingSystem.Models.Enums;
using AirportTicketBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Repository
{
    internal interface IBookingRepository
    {
        Task SaveBookingsAsync(List<Booking> bookings);
        Task<List<Booking>> GetBookingsByPassengerAsync(string passengerName);
        Task<bool> CancelBookingAsync(string passengerName, string flightId);
        Task<bool> ModifyBookingAsync(string passengerName, string flightId, string newClass);
        Task<decimal> GetUpdatedPriceAsync(string flightId, ClassType classType);
    }
}
