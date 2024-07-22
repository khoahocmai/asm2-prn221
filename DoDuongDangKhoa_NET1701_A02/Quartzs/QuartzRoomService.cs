using DataAccessObjects;
using DoDuongDangKhoa_NET1701_A02.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DoDuongDangKhoa_NET1701_A02.Quartzs
{
    public class QuartzRoomService : IQuartzRoomService
    {
        private readonly IHubContext<RoomHub> _hubContext;
        private readonly FuminiHotelManagementContext _context;
        private readonly ILogger<QuartzRoomService> _logger;

        public QuartzRoomService(IHubContext<RoomHub> hubContext, FuminiHotelManagementContext context, ILogger<QuartzRoomService> logger)
        {
            _hubContext = hubContext;
            _context = context;
            _logger = logger;
        }

        public async Task UpdateRoomStatusAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateOnly.FromDateTime(DateTime.Now);

                var bookingDetails = _context.BookingDetails
                    .Include(b => b.BookingReservation)
                    .Where(bd => bd.EndDate < currentTime)
                    .ToList();

                foreach (var bookingDetail in bookingDetails)
                {
                    bookingDetail.BookingReservation.BookingStatus = 2;
                    _context.BookingReservations.Update(bookingDetail.BookingReservation);
                }

                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveRoomStatus", "Room checked out");

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }


    }
}
