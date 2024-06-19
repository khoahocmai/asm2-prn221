using BusinessObjects;
using DataAccessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRoomInformationRepository
    {
        Task<List<RoomInformation>> GetRooms();
        Task<RoomInformation> GetRoomById(int id);
        Task AddRoom(RoomInformation room);
        Task UpdateRoom(RoomInformation room);
        Task RemoveRoom(int roomId);
        Task<List<RoomInformation>> GetRoomsByType(int roomTypeId);
        Task<List<RoomSearchResultDTO>> GetAvailableRooms(DateTime startDate, DateTime endDate);
        Task<RoomInformation> GetRoomByIdWithRoomType(int roomId);
    }
}
