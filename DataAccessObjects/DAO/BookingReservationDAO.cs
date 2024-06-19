using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects.DAO
{
    public class BookingReservationDAO
    {
        private static BookingReservationDAO instance = null;
        public static readonly object Lock = new object();
        private BookingReservationDAO() { }
        public static BookingReservationDAO Instance
        {
            get
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        instance = new BookingReservationDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<BookingReservation>> GetBookingReservations()
        {
            using var db = new FuminiHotelManagementContext();
            return await db.BookingReservations
                .Include(br => br.BookingDetails)
                .Include(br => br.Customer)
                .ToListAsync();
        } // Get list of all Booking Reservations

        public async Task<List<Customer>> GetCustomerList()
        {
            using var db = new FuminiHotelManagementContext();
            return await db.Customers.ToListAsync();
        } // Get list of all Customers

        public async Task<BookingReservation> GetBookingReservationById(int id)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.BookingReservations
                .Include(br => br.BookingDetails)
                .Include(br => br.Customer)
                .SingleOrDefaultAsync(br => br.BookingReservationId == id);
        } // Get Booking Reservation by ID

        public async Task<List<BookingReservation>> GetBookingReservationsByCustomerName(string name)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.BookingReservations
                .Include(br => br.BookingDetails)
                .Include(br => br.Customer)
                .Where(br => br.Customer.CustomerFullName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        } // Search Booking Reservations by Customer Name

        public async Task<List<BookingReservation>> GetBookingReservationsByBookingStatus(byte status)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.BookingReservations
                .Include(br => br.BookingDetails)
                .Include(br => br.Customer)
                .Where(br => br.BookingStatus == status)
                .ToListAsync();
        } // Search Booking Reservations by Booking Status

        public async Task SaveBookingReservation(BookingReservation bookingReservation)
        {
            using var db = new FuminiHotelManagementContext();
            await db.BookingReservations.AddAsync(bookingReservation);
            await db.SaveChangesAsync();
        } // Save a new Booking Reservation

        public async Task UpdateBookingReservation(BookingReservation bookingReservation)
        {
            using var db = new FuminiHotelManagementContext();
            db.BookingReservations.Update(bookingReservation);
            await db.SaveChangesAsync();
        } // Update an existing Booking Reservation

        public async Task RemoveBookingReservation(BookingReservation bookingReservation)
        {
            using var db = new FuminiHotelManagementContext();
            var existingReservation = await db.BookingReservations
                .SingleOrDefaultAsync(br => br.BookingReservationId == bookingReservation.BookingReservationId);
            if (existingReservation != null)
            {
                db.BookingReservations.Remove(existingReservation);
                await db.SaveChangesAsync();
            }
        } // Delete a Booking Reservation
    }
}
