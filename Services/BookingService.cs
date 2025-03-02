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
            if (!Enum.TryParse(selectedClass, true, out ClassType classType))
            {
                Console.WriteLine("Invalid class type. Please enter Economy, Business, or First.");
                return; 
            }
            if (!selectedFlight.TicketPrices.ContainsKey(classType.ToString()))
            {
                Console.WriteLine("Invalid class selection. Available options: Economy, Business, First.");
                return;
            }
            decimal ticketPrice = selectedFlight.TicketPrices[classType.ToString()];

            List<Booking> bookings = BookingRepository.GetAllBookings();
            Booking newBooking = new Booking
            {
                BookingId = Guid.NewGuid().ToString(),
                FlightId = selectedFlight.FlightId,
                PassengerName = passengerName,
                Class = classType,
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
        public void CancelBooking()
        {
            Console.Write("Enter your name: ");
            string passengerName = Console.ReadLine()?.Trim();

            Console.Write("Enter the Flight ID you want to cancel: ");
            string flightId = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(passengerName) || string.IsNullOrEmpty(flightId))
            {
                Console.WriteLine("Invalid input. Please enter valid details.");
                return;
            }

            bool isCancelled = _bookingRepository.CancelBooking(passengerName, flightId);
            if (isCancelled)
            {
                Console.WriteLine($"Booking for Flight {flightId} has been successfully canceled.");
            }
            else
            {
                Console.WriteLine("No matching booking found. Please check your details.");
            }
        }
        public void ModifyBooking()
        {
            Console.Write("Enter your name: ");
            string passengerName = Console.ReadLine()?.Trim();

            Console.Write("Enter the Flight ID of the booking you want to modify: ");
            string flightId = Console.ReadLine()?.Trim();

            Console.Write("Enter new class type (Economy, Business, First): ");
            string newClass = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(passengerName) || string.IsNullOrEmpty(flightId) || string.IsNullOrEmpty(newClass))
            {
                Console.WriteLine("Invalid input. Please enter valid details.");
                return;
            }

            bool isModified = _bookingRepository.ModifyBooking(passengerName, flightId, newClass);

            if (isModified)
            {
                Console.WriteLine($"Booking for Flight {flightId} has been successfully updated to {newClass} class.");
            }
            else
            {
                Console.WriteLine("Modification failed. Please check your details and try again.");
            }
        }
    }
}
