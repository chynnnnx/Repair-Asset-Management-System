using Client.ViewModels;
using Shared.DTOs;
using Client.Services.Interfaces;
using Blazored.LocalStorage;

namespace Client.Services.Implementations
{
    public class RoomService : BaseHttpService, IRoomService
    {
       public RoomService(HttpClient http, ILocalStorageService localStorage, ILogger<RoomService> logger)
      : base(http, localStorage, logger) { }

        public async Task<List<RoomViewModel>> GetAllRoomsAsync()
        {
            var dtos = await GetAsync<List<RoomDTO>>("api/Room") ?? new();
            return dtos.Select(x => x.ToViewModel()).ToList();
        }

        public async Task<bool> AddRoomAsync(RoomViewModel room)
        {
            return await PostAsync("api/Room", room.ToDTO());
        }

        public async Task<RoomViewModel?> GetRoomByIdAsync(int id)
        {
            var dto = await GetAsync<RoomDTO>($"api/Room/{id}");
            return dto?.ToViewModel();
        }

        public async Task<bool> UpdateRoomAsync(RoomViewModel room)
        {
            return await PutAsync("api/Room", room.ToDTO());
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            return await DeleteAsync($"api/Room/{id}");
        }
    }
}

