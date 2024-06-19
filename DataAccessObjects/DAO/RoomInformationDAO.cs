using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DTO;

namespace DataAccessObjects.DAO
{
    public class RoomInformationDAO
    {
        private static RoomInformationDAO instance = null;
        public static readonly object Lock = new object();
        private RoomInformationDAO() { }
        public static RoomInformationDAO Instance
        {
            get
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        instance = new RoomInformationDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<RoomInformation>> GetRooms()
        {
            using var db = new FuminiHotelManagementContext();
            return await db.RoomInformations.ToListAsync();
        }

        public async Task<RoomInformation> GetRoomById(int id)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.RoomInformations.SingleOrDefaultAsync(r => r.RoomId == id);
        }

        public async Task AddRoom(RoomInformation room)
        {
            using var db = new FuminiHotelManagementContext();
            db.RoomInformations.Add(room);
            await db.SaveChangesAsync();
        }

        public async Task UpdateRoom(RoomInformation room)
        {
            using var db = new FuminiHotelManagementContext();
            db.RoomInformations.Update(room);
            await db.SaveChangesAsync();
        }

        public async Task RemoveRoom(int roomId)
        {
            using var db = new FuminiHotelManagementContext();
            var room = await db.RoomInformations.SingleOrDefaultAsync(r => r.RoomId == roomId);
            if (room != null)
            {
                db.RoomInformations.Remove(room);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<RoomInformation>> GetRoomsByType(int roomTypeId)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.RoomInformations.Where(r => r.RoomTypeId == roomTypeId).ToListAsync();
        }

        public async Task<List<RoomSearchResultDTO>> GetAvailableRooms(DateTime startDate, DateTime endDate)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.RoomInformations
                .Include(r => r.RoomType)
                .Where(r => !db.BookingDetails
                    .Any(b => b.RoomId == r.RoomId &&
                              (DateOnly.FromDateTime(startDate) >= b.StartDate && DateOnly.FromDateTime(startDate) <= b.EndDate ||
                               DateOnly.FromDateTime(endDate) >= b.StartDate && DateOnly.FromDateTime(endDate) <= b.EndDate)))
                .Select(r => new RoomSearchResultDTO
                {
                    RoomId = r.RoomId,
                    RoomNumber = r.RoomNumber,
                    RoomDetailDescription = r.RoomDetailDescription,
                    RoomMaxCapacity = r.RoomMaxCapacity,
                    RoomTypeName = r.RoomType.RoomTypeName,
                    RoomStatus = r.RoomStatus,
                    RoomPricePerDay = r.RoomPricePerDay
                })
                .ToListAsync();
        }

        public async Task<RoomInformation> GetRoomByIdWithRoomType(int roomId)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.RoomInformations
                           .Include(r => r.RoomType)
                           .FirstOrDefaultAsync(r => r.RoomId == roomId);
        }
    }
}
