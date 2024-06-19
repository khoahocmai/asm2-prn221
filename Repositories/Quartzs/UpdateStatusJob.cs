using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Repositories.Quartzs
{
    [DisallowConcurrentExecution]
    public class UpdateStatusJob : IJob
    {
        private readonly IRoomInformationRepository _roomRepo;
        private readonly IBookingDetailRepository _bookingDetailRepo;
        private readonly IScheduler _scheduler;
        private readonly ILogger<UpdateStatusJob> _logger;

        public UpdateStatusJob(
           IRoomInformationRepository repository,
           IBookingDetailRepository bookingDetailRepo,
           IScheduler scheduler,
           ILogger<UpdateStatusJob> logger
       )
        {
            _roomRepo = repository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                //DateOnly currentDate = new DateOnly();
                //var data = await _bookingDetailRepo.GetBookingDetails();
                //foreach (var item in data)
                //{
                //    if (currentDate > item.EndDate)
                //    {
                //        var room = await _roomRepo.GetRoomById(item.RoomId);
                //        room.RoomStatus = 1;
                //    }
                //}
                var data = await _roomRepo.GetRooms();
                foreach (var item in data)
                {
                    _logger.LogInformation($"Room number:{item.RoomNumber}");
                }
                //return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                //return false;
            }
        }
    }
}
