// Pages/CustomerBooking/BookingDetail.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DataAccessObjects;
using BusinessObjects;
using DataAccessObjects.DTO;

namespace DoDuongDangKhoa_NET1701_A02.Pages.CustomerBooking
{
    public class BookingDetailModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingDetailModel(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookingDetailDTO BookingDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var bookingReservation = await _context.BookingReservations
                .FirstOrDefaultAsync(b => b.BookingReservationId == id);

            if (bookingReservation == null)
            {
                return NotFound();
            }

            var bookingDetails = await _context.BookingDetails
                .Where(bd => bd.BookingReservationId == id)
                .Join(_context.RoomInformations,
                      bd => bd.RoomId,
                      r => r.RoomId,
                      (bd, r) => new BookingDetailDTO.RoomBookingDetail
                      {
                          RoomNumber = r.RoomNumber,
                          StartDate = bd.StartDate.ToDateTime(TimeOnly.MinValue),
                          EndDate = bd.EndDate.ToDateTime(TimeOnly.MinValue),
                          ActualPrice = (decimal)bd.ActualPrice
                      })
                .ToListAsync();

            BookingDetail = new BookingDetailDTO
            {
                BookingReservationId = bookingReservation.BookingReservationId,
                BookingDate = DateTime.Parse(bookingReservation.BookingDate.ToString()),
                TotalPrice = bookingReservation.TotalPrice,
                BookingStatus = bookingReservation.BookingStatus,
                RoomDetails = bookingDetails
            };

            return Page();
        }
    }
}
