using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBookingDetailRepository
    {
        Task<List<BookingDetail>> GetBookingDetails();
        Task<BookingDetail> GetBookingDetailById(int bookingReservationId, int roomId);
        Task<List<BookingDetail>> GetBookingDetailsByReservationId(int bookingReservationId);
        Task SaveBookingDetail(BookingDetail bookingDetail);
        Task UpdateBookingDetail(BookingDetail bookingDetail);
        Task RemoveBookingDetail(int bookingReservationId, int roomId);
    }
}
