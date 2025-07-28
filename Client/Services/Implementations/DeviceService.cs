using System.Text.Json;
using Shared.DTOs;
using Client.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;

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

        public async Task<List<DeviceDTO>> GetAllDevicesAsync()
        {
            var stream = await _http.GetStreamAsync("api/Device");
            return await JsonSerializer.DeserializeAsync<List<DeviceDTO>>(stream, _jsonOptions) ?? new();
        }

        public async Task<bool> AddDeviceAsync(DeviceDTO deviceDto)
        {
            return await PostAsync("api/Device", deviceDto);
        }

     

        public async Task<bool> UpdateDeviceAsync(DeviceDTO deviceDto)
        {
            return await PutAsync("api/Device", deviceDto);
        }

        public async Task<bool> DeleteDeviceAsync(int deviceId)
        {
            return await DeleteAsync($"api/Device/{deviceId}");
        }
    }
}
