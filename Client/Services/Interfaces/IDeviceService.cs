using Shared.DTOs;

namespace Client.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<List<DeviceDTO>> GetAllDevicesAsync();
        Task<bool> AddDeviceAsync(DeviceDTO deviceDto);
        Task<bool> UpdateDeviceAsync(DeviceDTO deviceDto);
        Task<bool> DeleteDeviceAsync(int deviceId);
    }
}
