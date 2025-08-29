using Client.ViewModels;

namespace Client.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomViewModel>> GetAllRoomsAsync();
        Task<bool> AddRoomAsync(RoomViewModel room);
        Task<RoomViewModel?> GetRoomByIdAsync(int id);
        Task<bool> UpdateRoomAsync(RoomViewModel room);
        Task<bool> DeleteRoomAsync(int id);
    }
}
