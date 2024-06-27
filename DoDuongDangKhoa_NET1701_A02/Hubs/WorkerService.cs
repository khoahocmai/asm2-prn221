using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using DoDuongDangKhoa_NET1701_A02.Hubs;
using DataAccessObjects;
using BusinessObjects;
using DoDuongDangKhoa_NET1701_A02.Quartzs;

namespace DoDuongDangKhoa_NET1701_A02
{
    public class WorkerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(IServiceScopeFactory serviceScopeFactory, ILogger<WorkerService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var roomUpdateStatusService = scope.ServiceProvider.GetRequiredService<IQuartzRoomService>();
                    await roomUpdateStatusService.UpdateRoomStatusAsync(stoppingToken);
                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
