using Shared.DTOs;

namespace Client.Services.Interfaces
{
    public interface IDeviceLogService
    {
        Task<List<DeviceLogDTO>> GetAllLogs();
    }
}
