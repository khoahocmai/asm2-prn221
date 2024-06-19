using BusinessObjects;
using DataAccessObjects.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        public Task<List<BookingDetail>> GetBookingDetails()
            => BookingDetailDAO.Instance.GetBookingDetails();

        public Task<BookingDetail> GetBookingDetailById(int bookingReservationId, int roomId)
            => BookingDetailDAO.Instance.GetBookingDetailById(bookingReservationId, roomId);

        public Task<List<BookingDetail>> GetBookingDetailsByReservationId(int bookingReservationId)
            => BookingDetailDAO.Instance.GetBookingDetailsByReservationId(bookingReservationId);

        public Task SaveBookingDetail(BookingDetail bookingDetail)
            => BookingDetailDAO.Instance.SaveBookingDetail(bookingDetail);

        public Task UpdateBookingDetail(BookingDetail bookingDetail)
            => BookingDetailDAO.Instance.UpdateBookingDetail(bookingDetail);

        public Task RemoveBookingDetail(int bookingReservationId, int roomId)
            => BookingDetailDAO.Instance.RemoveBookingDetail(bookingReservationId, roomId);
    }
}
