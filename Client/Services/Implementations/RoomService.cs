using Client.Services.Interfaces;
using Shared.DTOs;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;

namespace Client.Services.Implementations
{
    public class RoomService : BaseHttpService, IRoomService
    {
        public RoomService(HttpClient http, ILocalStorageService localStorage, ILogger<RoomService> logger)
            : base(http, localStorage, logger) { }

        public async Task<List<RoomDTO>> GetAllRoomsAsync()
        {
            return await GetAsync<List<RoomDTO>>("api/Room") ?? new();
        }

        public async Task<bool> AddRoomAsync(RoomDTO roomDto)
        {
            return await PostAsync("api/Room", roomDto);
        }

        public async Task<RoomDTO?> GetRoomByIdAsync(int id)
        {
            return await GetAsync<RoomDTO>($"api/Room/{id}");
        }

        public async Task<bool> UpdateRoomAsync(RoomDTO roomDto)
        {
            return await PutAsync("api/Room", roomDto);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            return await DeleteAsync($"api/Room/{id}");
        }
    }
}
