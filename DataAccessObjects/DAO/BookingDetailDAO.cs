using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DAO
{
    public class BookingDetailDAO
    {
        private static BookingDetailDAO instance = null;
        public static readonly object Lock = new object();
        private BookingDetailDAO() { }
        public static BookingDetailDAO Instance
        {
            get
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<BookingDetail>> GetBookingDetails()
        {
            using var db = new FuminiHotelManagementContext();
            return await db.BookingDetails
                .Include(bd => bd.BookingReservation)
                .Include(bd => bd.Room)
                .ToListAsync();
        } // Get list of all Booking Details

        public async Task<BookingDetail> GetBookingDetailById(int bookingReservationId, int roomId)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.BookingDetails
                .Include(bd => bd.BookingReservation)
                .Include(bd => bd.Room)
                .SingleOrDefaultAsync(bd => bd.BookingReservationId == bookingReservationId && bd.RoomId == roomId);
        } // Get Booking Detail by BookingReservationId and RoomId

        public async Task<List<BookingDetail>> GetBookingDetailsByReservationId(int bookingReservationId)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.BookingDetails
                .Include(bd => bd.BookingReservation)
                .Include(bd => bd.Room)
                .Where(bd => bd.BookingReservationId == bookingReservationId)
                .ToListAsync();
        } // Get Booking Details by BookingReservationId

        public async Task SaveBookingDetail(BookingDetail bookingDetail)
        {
            using var db = new FuminiHotelManagementContext();
            await db.BookingDetails.AddAsync(bookingDetail);
            await db.SaveChangesAsync();
        } // Save a new Booking Detail

        public async Task UpdateBookingDetail(BookingDetail bookingDetail)
        {
            using var db = new FuminiHotelManagementContext();
            db.BookingDetails.Update(bookingDetail);
            await db.SaveChangesAsync();
        } // Update an existing Booking Detail

        public async Task RemoveBookingDetail(int bookingReservationId, int roomId)
        {
            using var db = new FuminiHotelManagementContext();
            var existingDetail = await db.BookingDetails
                .SingleOrDefaultAsync(bd => bd.BookingReservationId == bookingReservationId && bd.RoomId == roomId);
            if (existingDetail != null)
            {
                db.BookingDetails.Remove(existingDetail);
                await db.SaveChangesAsync();
            }
        } // Delete a Booking Detail
    }
}

