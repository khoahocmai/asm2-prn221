namespace DoDuongDangKhoa_NET1701_A02.Quartzs
{
    public interface IQuartzRoomService
    {
        Task UpdateRoomStatusAsync(CancellationToken stoppingToken);
    }
}
