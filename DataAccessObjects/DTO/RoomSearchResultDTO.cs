namespace DataAccessObjects.DTO
{
    public class RoomSearchResultDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomDetailDescription { get; set; }
        public int? RoomMaxCapacity { get; set; }
        public string RoomTypeName { get; set; }
        public byte? RoomStatus { get; set; }
        public decimal? RoomPricePerDay { get; set; }
    }
}
