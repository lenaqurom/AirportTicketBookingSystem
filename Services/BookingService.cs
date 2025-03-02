using AirportTicketBookingSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    internal class BookingService
    {
        private readonly FlightRepository _flightRepository;
        private readonly BookingRepository _bookingRepository;
        public BookingService()
        {
            _flightRepository = new FlightRepository();
            _bookingRepository = new BookingRepository();
        }
        public void BookFlight(string flightId, string selectedClass, string passengerName)
        {
            List<Flight> flights = _flightRepository.GetAllFlights();
            Flight? selectedFlight = flights.FirstOrDefault(f => f.FlightId == flightId);

            if(selectedFlight == null)
            {
                Console.WriteLine("Invalid Flight ID. Please try again.");
                return;
            }
            if(!selectedFlight.TicketPrices.ContainsKey(selectedClass))
            {
                Console.WriteLine("Invalid class selection. Available options: Economy, Business, First.");
                return;
            }
            decimal ticketPrice = selectedFlight.TicketPrices[selectedClass];

            List<Booking> bookings = BookingRepository.GetAllBookings();
            Booking newBooking = new Booking
            {
                BookingId = Guid.NewGuid().ToString(),
                FlightId = selectedFlight.FlightId,
                PassengerName = passengerName,
                Class = selectedClass,
                Price = ticketPrice,
                BookingDate = DateTime.UtcNow
            };
            bookings.Add(newBooking);
            _bookingRepository.SaveBookings(bookings);
            Console.WriteLine($"Booking successful! {passengerName} booked {selectedClass} class on flight {flightId} for {ticketPrice:C}.");
        }
        public void ViewPersonalBookings()
        {
            Console.Write("Enter your name: ");
            string passengerName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(passengerName))
            {
                Console.WriteLine("Invalid input. Please enter a valid name.");
                return;
            }
            List<Booking> bookings = _bookingRepository.GetBookingsByPassenger(passengerName);
            if(bookings.Count == 0)
            {
                Console.WriteLine($"No bookings found for {passengerName}.");
            }
            else
            {
                Console.WriteLine($"\nBookings for {passengerName}:");
                foreach(var booking in bookings)
                {
                    Console.WriteLine($"- Flight: {booking.FlightId}, Class: {booking.Class}, Price: ${booking.Price}");
                }
            }

        }
    }
}
