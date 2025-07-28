using Client.Services;
using Shared.DTOs;
namespace Client.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomDTO>> GetAllRoomsAsync();
        Task<bool> AddRoomAsync(RoomDTO roomDto);
        Task<RoomDTO?> GetRoomByIdAsync(int id);
        Task<bool> UpdateRoomAsync(RoomDTO roomDto);
        Task<bool> DeleteRoomAsync(int id);
    }
}
