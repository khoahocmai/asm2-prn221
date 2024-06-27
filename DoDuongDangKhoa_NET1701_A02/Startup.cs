using DataAccessObjects;
using DoDuongDangKhoa_NET1701_A02.Hubs;
using DoDuongDangKhoa_NET1701_A02.Quartzs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Repositories;

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

            // Add repositories to the container.
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRoomInformationRepository, RoomInformationRepository>();
            services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();
            services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();

            // Add Quartz room service
            services.AddScoped<IQuartzRoomService, QuartzRoomService>();
            // Register WorkerService as a hosted service
            services.AddHostedService<WorkerService>();

            // Add authentication services
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // Path to the login page
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // DI for DbContext
            services.AddDbContext<FuminiHotelManagementContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("SqlConnection")));
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
