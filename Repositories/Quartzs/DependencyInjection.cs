using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Repositories.Quartzs
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobkey = JobKey.Create(nameof(LoggingBackgroundJob));
                options
                    .AddJob<LoggingBackgroundJob>(jobkey)
                    .AddTrigger(trigger =>
                        trigger
                            .ForJob(jobkey)
                            .WithSimpleSchedule(schedule =>
                            {
                                schedule.WithIntervalInSeconds(5).RepeatForever();
                            }));
            });
            services.AddQuartzHostedService();
        }
    }
}
