using Blazored.LocalStorage;
using Client.Services.Interfaces;
using Client.ViewModels;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace Client.Services.Implementations
{
    public class DeviceLogService: BaseHttpService, IDeviceLogService
    {
        public DeviceLogService(HttpClient http, ILogger<DeviceLogService> logger, ILocalStorageService localStorage) : base(http, localStorage, logger)
        {

        }

        public async Task <List<DeviceLogViewModel>> GetAllLogs()
        {
            var dtos = await GetAsync<List<DeviceLogDTO>>("api/DeviceLog") ?? new();
            return dtos.Select(x => x.ToViewModel()).ToList();

        }

    }
}
