using Shared.DTOs;

namespace projServer.Services.Interfaces
{
    public interface IDeviceLogService
    {
        Task<IEnumerable<DeviceLogDTO>> GetLogs(int deviceId);
        Task<IEnumerable<DeviceLogDTO>> GetAllLogs();
   
    }
}
