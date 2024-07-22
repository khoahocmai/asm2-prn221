using BusinessObjects;

namespace DataAccessObjects.DTO
{
    public class BookingHistoryDTO
    {
        public int BookingReservationId { get; set; }

        public DateOnly? BookingDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? BookingStatus { get; set; }

        public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
    }
}
