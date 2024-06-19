using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessObjects;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DoDuongDangKhoa_NET1701_A02.Pages.CustomerBooking
{
    public class BookingHistoryModel : PageModel
    {
        private readonly FuminiHotelManagementContext _context;

        public BookingHistoryModel(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public List<BookingReservation> Bookings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst("CustomerID");
            int customerId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

            Bookings = await _context.BookingReservations
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return Page();
        }
    }
}
