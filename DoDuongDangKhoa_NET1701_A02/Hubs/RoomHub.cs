using Microsoft.AspNetCore.SignalR;

namespace DoDuongDangKhoa_NET1701_A02.Hubs
{
    public class RoomHub : Hub
    {
        public RoomHub() { }

        public async Task SendRoomStatus(string message)
        {
            await Clients.All.SendAsync("ReceiveRoomStatus", message);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task UpdateBookingStatus(int bookingId, int status)
        {
            await Clients.All.SendAsync("UpdateBookingStatus", bookingId, status);
        }
    }

}
