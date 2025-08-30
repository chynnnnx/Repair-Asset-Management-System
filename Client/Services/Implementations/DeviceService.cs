using System.Text.Json;
using Shared.DTOs;
using Client.Services.Interfaces;
using Blazored.LocalStorage;
using Client.ViewModels;

namespace Client.Services.Implementations
{
    public class DeviceService : BaseHttpService, IDeviceService
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public DeviceService(
            HttpClient http,
            ILocalStorageService localStorage,
            JsonSerializerOptions jsonOptions,
            ILogger<DeviceService> logger)
            : base(http, localStorage, logger)
        {
            _jsonOptions = jsonOptions;
        }

        public async Task<List<DeviceViewModel>> GetAllDevicesAsync()
        {
            var dtos = await GetAsync<List<DeviceDTO>>("api/Device") ?? new();
            return dtos.Select(x => x.ToViewModel()).ToList();
        }

        public async Task<bool> AddDeviceAsync(DeviceViewModel device)
        {
            return await PostAsync("api/Device", device.ToDTO());
        }

     

        public async Task<bool> UpdateDeviceAsync(DeviceViewModel device)
        {
            return await PutAsync("api/Device", device.ToDTO());
        }

        public async Task<bool> DeleteDeviceAsync(int deviceId)
        {
            return await DeleteAsync($"api/Device/{deviceId}");
        }
    }
}
