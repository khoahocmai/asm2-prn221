using DataAccessObjects;
using DoDuongDangKhoa_NET1701_A02.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Repositories;
using Repositories.Quartzs;

namespace DoDuongDangKhoa_NET1701_A02
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddRazorPages();

            // Add SignalR to the container.
            services.AddSignalR();

            // Add service to the container.
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRoomInformationRepository, RoomInformationRepository>();
            services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();
            services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();

            // Add Quartz services via the static method
            DependencyInjection.AddInfrastructure(services);

            // Add authentication services
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // Đường dẫn tới trang đăng nhập
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // Add Quartz services 
            //services.AddQuartz(q =>
            //{
            //    q.UseMicrosoftDependencyInjectionJobFactory();
            //    // q.UseMicrosoftDependencyInjectionScopedJobFactory();

            //    // Create a "key" for the job
            //    var jobKey = new JobKey("UpdateStatus");

            //    // Register the job with the DI container
            //    q.AddJob<UpdateStatusJob>(opts => opts.WithIdentity(jobKey));

            //    // Create a trigger for the job
            //    q.AddTrigger(opts => opts
            //        .ForJob(jobKey) // link to the HelloWorldJob
            //        .WithIdentity("UpdateStatusJob-trigger") // give the trigger a unique name
            //        .WithCronSchedule(_configuration.GetSection("CronJob")?.Value)); // get cron schedule from configuration
            //});
            //services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            // DI
            services.AddDbContext<FuminiHotelManagementContext>(options => options.UseSqlServer(_configuration.GetConnectionString("SqlConnection")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Use authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<RoomHub>("/RoomHub");
            });
        }
    }
}
