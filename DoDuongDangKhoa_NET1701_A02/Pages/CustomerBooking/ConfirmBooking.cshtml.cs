using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using BusinessObjects;
using Repositories;

namespace DoDuongDangKhoa_NET1701_A02.Pages.CustomerBooking
{
    public class ConfirmBookingModel : PageModel
    {
        private readonly IRoomInformationRepository _roomRepo;
        private readonly IBookingReservationRepository _bookingReservationRepo;
        private readonly IBookingDetailRepository _bookingDetailRepo;

        public ConfirmBookingModel(IRoomInformationRepository roomInformationRepository, IBookingReservationRepository bookingReservationRepository, IBookingDetailRepository bookingDetailRepo)
        {
            _roomRepo = roomInformationRepository;
            _bookingReservationRepo = bookingReservationRepository;
            _bookingDetailRepo = bookingDetailRepo;
        }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        [BindProperty]
        public int RoomId { get; set; }

        [BindProperty]
        public string? RoomNumber { get; set; }

        [BindProperty]
        public string? RoomTypeName { get; set; }

        [BindProperty]
        public decimal RoomPricePerDay { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var room = await _roomRepo.GetRoomByIdWithRoomType(RoomId);

            if (room == null)
            {
                return NotFound();
            }

            RoomNumber = room.RoomNumber;
            RoomTypeName = room.RoomType.RoomTypeName;
            RoomPricePerDay = (decimal) room.RoomPricePerDay;

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userIdClaim = User.FindFirst("CustomerID");
            int customerId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            var bookingReservation = new BookingReservation
            {
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                TotalPrice = await CalculateTotalPrice(RoomId, StartDate, EndDate),
                CustomerId = customerId,
                BookingStatus = 1
            };

            
            await _bookingReservationRepo.SaveBookingReservation(bookingReservation);

            var bookingDetail = new BookingDetail
            {
                BookingReservationId = bookingReservation.BookingReservationId,
                RoomId = RoomId,
                StartDate = DateOnly.FromDateTime(StartDate),
                EndDate = DateOnly.FromDateTime(EndDate),
                ActualPrice = await CalculateTotalPrice(RoomId, StartDate, EndDate)
            };

            await _bookingDetailRepo.SaveBookingDetail(bookingDetail);

            return RedirectToPage("./BookingSuccess");
        }

        private async Task<decimal> CalculateTotalPrice(int roomId, DateTime startDate, DateTime endDate)
        {
            var room = await _roomRepo.GetRoomById(roomId);
            if (room == null)
            {
                throw new Exception("Room not found");
            }
            int numberOfDays = (endDate - startDate).Days;
            return (decimal)(numberOfDays * room.RoomPricePerDay);
        }
    }
}
