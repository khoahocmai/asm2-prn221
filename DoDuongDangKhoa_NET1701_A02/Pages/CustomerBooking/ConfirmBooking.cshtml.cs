using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects;
using Repositories;
using Microsoft.AspNetCore.SignalR;
using DoDuongDangKhoa_NET1701_A02.Hubs;

namespace DoDuongDangKhoa_NET1701_A02.Pages.CustomerBooking
{
    public class ConfirmBookingModel : PageModel
    {
        private readonly IRoomInformationRepository _roomRepo;
        private readonly IBookingReservationRepository _bookingReservationRepo;
        private readonly IBookingDetailRepository _bookingDetailRepo;
        private readonly IHubContext<RoomHub> _hubContext;

        public ConfirmBookingModel(
            IRoomInformationRepository roomInformationRepository,
            IBookingReservationRepository bookingReservationRepository,
            IBookingDetailRepository bookingDetailRepo,
            IHubContext<RoomHub> hubContext
            )
        {
            _roomRepo = roomInformationRepository;
            _bookingReservationRepo = bookingReservationRepository;
            _bookingDetailRepo = bookingDetailRepo;
            _hubContext = hubContext;
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


        //public IActionResult OnGet(int roomId, DateTime startDate, DateTime endDate, string roomNumber, string roomTypeName, decimal roomPricePerDay)
        //{
        //    // Populate the properties with the passed parameters
        //    RoomId = roomId;
        //    StartDate = startDate;
        //    EndDate = endDate;
        //    RoomNumber = roomNumber;
        //    RoomTypeName = roomTypeName;
        //    RoomPricePerDay = roomPricePerDay;

        //    // Check if the room ID is valid to display the confirmation details
        //    if (RoomId <= 0)
        //    {
        //        return RedirectToPage("/Error");
        //    }

        //    return Page();
        //}

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
            RoomPricePerDay = (decimal)room.RoomPricePerDay;

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
            await _hubContext.Clients.All.SendAsync("ReceiveRoomStatus", bookingDetail.RoomId, "Available");

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
