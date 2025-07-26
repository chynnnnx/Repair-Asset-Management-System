using projServer.Services.Implementations;
using Shared.DTOs;

namespace projServer.Services.Interfaces
{
    public interface IRoomService
    {
        Task AddRoomAsync(RoomDTO roomDto);
        Task<List<RoomDTO>> GetAllRoomsAsync();
        Task<RoomDTO?> GetRoomByIdAsync(int id);
        Task UpdateRoomAsync(RoomDTO roomDto);
        Task DeleteRoomAsync(int id);
    }
}
