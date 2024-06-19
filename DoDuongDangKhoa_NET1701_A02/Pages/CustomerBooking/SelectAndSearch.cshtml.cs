using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using DataAccessObjects.DTO;
using Repositories;

namespace DoDuongDangKhoa_NET1701_A02.Pages.CustomerBooking
{
    public class SelectAndSearchModel : PageModel
    {
        private readonly IRoomInformationRepository _roomRepo;

        public SelectAndSearchModel(IRoomInformationRepository roomInformationRepository)
        {
            _roomRepo = roomInformationRepository;
        }

        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public bool ShowResults { get; set; } = false;
        public List<RoomSearchResultDTO> AvailableRooms { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (EndDate == null)
            {
                ModelState.AddModelError("EndDate", "End date is required");
            }
            if (StartDate == null)
            {
                ModelState.AddModelError("StartDate", "Start date is required");
            }
            if (StartDate != null && EndDate != null)
            {
                var end = EndDate.Date;
                var start = StartDate.Date;
                if (start < DateTime.Today)
                {
                    ModelState.AddModelError("StartDate", "Renting date cannot be in the past");
                }
                if (end < DateTime.Today)
                {
                    ModelState.AddModelError("EndDate", "Renting date cannot be in the past");
                }
                if (end < start )
                {
                    ModelState.AddModelError("EndDate", "End date cannot before Start date");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            ShowResults = true;

            AvailableRooms = await _roomRepo.GetAvailableRooms(StartDate, EndDate);

            return Page();
        }
    }
}
