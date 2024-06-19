using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface IBookingReservationRepository
    {
        Task<List<BookingReservation>> GetBookingReservations();
        Task<List<Customer>> GetCustomerList();
        Task<BookingReservation> GetBookingReservationById(int id);
        Task<List<BookingReservation>> GetBookingReservationsByCustomerName(string name);
        Task<List<BookingReservation>> GetBookingReservationsByBookingStatus(byte status);
        Task SaveBookingReservation(BookingReservation bookingReservation);
        Task UpdateBookingReservation(BookingReservation bookingReservation);
        Task RemoveBookingReservation(BookingReservation bookingReservation);
    }
}
