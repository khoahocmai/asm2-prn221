namespace DataAccessObjects.DTO
{
    public class BookingDetailDTO
    {
        public int BookingReservationId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public byte? BookingStatus { get; set; }
        public List<RoomBookingDetail>? RoomDetails { get; set; }

        public class RoomBookingDetail
        {
            public string? RoomNumber { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public decimal? ActualPrice { get; set; }
        }
    }
}
