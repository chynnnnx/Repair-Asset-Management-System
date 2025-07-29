using projServer.Services.Implementations;
using Shared.DTOs;

namespace projServer.Services.Interfaces
{
    public interface IRoomService
    {
        Task<bool> AddRoomAsync(RoomDTO roomDto);
        Task<bool> UpdateRoomAsync(RoomDTO roomDto);
        Task<bool> DeleteRoomAsync(int id);
        Task<List<RoomDTO>> GetAllRoomsAsync();
        Task<RoomDTO?> GetRoomByIdAsync(int id);

    }
}
