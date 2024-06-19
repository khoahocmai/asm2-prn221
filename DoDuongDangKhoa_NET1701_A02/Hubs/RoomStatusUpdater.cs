using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using DoDuongDangKhoa_NET1701_A02.Hubs;
using DataAccessObjects;
using BusinessObjects;

namespace DoDuongDangKhoa_NET1701_A02
{
    public class RoomStatusUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RoomStatusUpdater> _logger;
        private readonly IHubContext<RoomHub> _hubContext;

        public RoomStatusUpdater(IServiceProvider serviceProvider, ILogger<RoomStatusUpdater> logger, IHubContext<RoomHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("RoomStatusUpdater running at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<FuminiHotelManagementContext>();

                    var currentDateTime = DateTime.Now;

                    var roomsToUpdate = await context.RoomInformations
                        .Include(r => r.BookingDetails)
                        .Where(r => r.RoomStatus == 2 && r.BookingDetails.Any(b => b.EndDate.ToDateTime(TimeOnly.MinValue) <= currentDateTime))
                        .ToListAsync(stoppingToken);

                    foreach (var room in roomsToUpdate)
                    {
                        room.RoomStatus = 1; // Cập nhật trạng thái phòng
                        context.RoomInformations.Update(room);
                        await _hubContext.Clients.All.SendAsync("ReceiveRoomStatus", room.RoomId, room.RoomStatus);
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
                await _hubContext.Clients.All.SendAsync("ReceiveRoomStatus", "Room checked out");

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Thực hiện mỗi 5 phút
            }
        }
    }
}
