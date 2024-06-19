using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Quartzs
{
    [DisallowConcurrentExecution]
    public class LoggingBackgroundJob : IJob
    {
        private readonly ILogger<LoggingBackgroundJob> _logger;
        private readonly IRoomInformationRepository _roomRepo;
        public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger, IRoomInformationRepository roomRepo)
        {
            _logger = logger;
            _roomRepo = roomRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var data = await _roomRepo.GetRooms();
                foreach (var item in data)
                {
                    _logger.LogInformation($"Data LastName:{item.RoomNumber}");
                }
                //return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:{ex}");
                //return false;
            }
        }
    }
}
