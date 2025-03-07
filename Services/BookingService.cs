using AirportTicketBookingSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Services.Interfaces;
using AirportTicketBookingSystem.Models.Enums;

namespace AirportTicketBookingSystem.Services
{
    internal class BookingService : IBookingService
    {
        private readonly FlightRepository _flightRepository;
        private readonly BookingRepository _bookingRepository;
        public BookingService()
        {
            _flightRepository = new FlightRepository();
            _bookingRepository = new BookingRepository();
        }
        public async Task BookFlightAsync(string flightId, string selectedClass, string passengerName)
        {
            List<Flight> flights = await _flightRepository.GetAllFlightsAsync();
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

            Passenger passenger = new Passenger(passengerName);


            List<Booking> bookings = await BookingRepository.GetAllBookingsAsync();
            Booking newBooking = new Booking(passenger, selectedFlight.FlightId, classType, ticketPrice);

            bookings.Add(newBooking);
            await _bookingRepository.SaveBookingsAsync(bookings);
            Console.WriteLine($"Booking successful! {passengerName} booked {selectedClass} class on flight {flightId} for {ticketPrice:C}.");
        }
        public async Task ViewPersonalBookingsAsync()
        {
            Console.Write("Enter your name: ");
            string passengerName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(passengerName))
            {
                Console.WriteLine("Invalid input. Please enter a valid name.");
                return;
            }
            List<Booking> bookings = await _bookingRepository.GetBookingsByPassengerAsync(passengerName);
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
        public async Task CancelBookingAsync()
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

            bool isCancelled = await _bookingRepository.CancelBookingAsync(passengerName, flightId);
            if (isCancelled)
            {
                Console.WriteLine($"Booking for Flight {flightId} has been successfully canceled.");
            }
            else
            {
                Console.WriteLine("No matching booking found. Please check your details.");
            }
        }
        public async Task ModifyBookingAsync()
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

            bool isModified = await _bookingRepository.ModifyBookingAsync(passengerName, flightId, newClass);

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
