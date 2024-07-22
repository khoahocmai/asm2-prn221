using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessObjects;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessObjects.DTO;

namespace DoDuongDangKhoa_NET1701_A02.Pages.CustomerBooking
{
    public class BookingHistoryModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingHistoryModel(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public List<BookingHistoryDTO> Bookings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst("CustomerID");
            int customerId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            var bookingReservations = await _context.BookingReservations
            .Where(b => b.CustomerId == customerId)
            .OrderByDescending(b => b.BookingDate)
            .Include(b => b.BookingDetails)
            .ToListAsync();

            Bookings = bookingReservations.Select(booking => new BookingHistoryDTO
            {
                BookingReservationId = booking.BookingReservationId,
                BookingDate = booking.BookingDate,
                TotalPrice = booking.TotalPrice,
                BookingStatus = booking.BookingStatus switch
                {
                    1 => "Pending",
                    2 => "Finish",
                    _ => "Unknown"
                },
                BookingDetails = booking.BookingDetails
            }).ToList();

            return Page();
        }

        public async Task<JsonResult> OnGetFetchUpdatedDataAsync()
        {
            var userIdClaim = User.FindFirst("CustomerID");
            int customerId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            var bookingReservations = await _context.BookingReservations
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .Include(b => b.BookingDetails)
                .ToListAsync();

            var bookingData = bookingReservations.Select(b => new
            {
                bookingDate = b.BookingDate,
                totalPrice = b.TotalPrice,
                bookingStatus = b.BookingStatus == 1 ? "Pending" : (b.BookingStatus == 2 ? "Finish" : "Unknown"),
                bookingReservationId = b.BookingReservationId
            });

            return new JsonResult(bookingData);
        }
    }
}
