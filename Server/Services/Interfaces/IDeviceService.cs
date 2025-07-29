using Shared.DTOs;

namespace projServer.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<bool> AddDeviceAsync(DeviceDTO deviceDto);
        Task<List<DeviceDTO>> GetAllDeviceAsync();
        Task<bool> UpdateDeviceAsync(DeviceDTO deviceDto);
        Task<bool> DeleteDeviceAsync(int id);
    }
}
