using BusinessObjects;
using DataAccessObjects.DAO;
using DataAccessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RoomInformationRepository : IRoomInformationRepository
    {
        public Task<List<RoomInformation>> GetRooms()
            => RoomInformationDAO.Instance.GetRooms();

        public Task<RoomInformation> GetRoomById(int id)
            => RoomInformationDAO.Instance.GetRoomById(id);

        public Task AddRoom(RoomInformation room)
            => RoomInformationDAO.Instance.AddRoom(room);

        public Task UpdateRoom(RoomInformation room)
            => RoomInformationDAO.Instance.UpdateRoom(room);

        public Task RemoveRoom(int roomId)
            => RoomInformationDAO.Instance.RemoveRoom(roomId);

        public Task<List<RoomInformation>> GetRoomsByType(int roomTypeId)
            => RoomInformationDAO.Instance.GetRoomsByType(roomTypeId);

        public Task<List<RoomSearchResultDTO>> GetAvailableRooms(DateTime startDate, DateTime endDate)
            => RoomInformationDAO.Instance.GetAvailableRooms(startDate, endDate);

        public Task<RoomInformation> GetRoomByIdWithRoomType(int roomId)
            => RoomInformationDAO.Instance.GetRoomByIdWithRoomType(roomId);
    }
}
