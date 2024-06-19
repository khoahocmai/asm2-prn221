using BusinessObjects;
using DataAccessObjects.DAO;
using System.Collections.Generic;

namespace Repositories
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        public Task<List<BookingReservation>> GetBookingReservations()
            => BookingReservationDAO.Instance.GetBookingReservations();

        public Task<List<Customer>> GetCustomerList()
            => BookingReservationDAO.Instance.GetCustomerList();

        public Task<BookingReservation> GetBookingReservationById(int id)
            => BookingReservationDAO.Instance.GetBookingReservationById(id);

        public Task<List<BookingReservation>> GetBookingReservationsByCustomerName(string name)
            => BookingReservationDAO.Instance.GetBookingReservationsByCustomerName(name);

        public Task<List<BookingReservation>> GetBookingReservationsByBookingStatus(byte status)
            => BookingReservationDAO.Instance.GetBookingReservationsByBookingStatus(status);

        public Task SaveBookingReservation(BookingReservation bookingReservation)
            => BookingReservationDAO.Instance.SaveBookingReservation(bookingReservation);

        public Task UpdateBookingReservation(BookingReservation bookingReservation)
            => BookingReservationDAO.Instance.UpdateBookingReservation(bookingReservation);

        public Task RemoveBookingReservation(BookingReservation bookingReservation)
            => BookingReservationDAO.Instance.RemoveBookingReservation(bookingReservation);
    }
}
