using Shared.DTOs;

namespace projServer.Services.Interfaces
{
    public interface IDeviceService
    {
        Task AddDeviceAsync(DeviceDTO deviceDto);
        Task<List<DeviceDTO>> GetAllDeviceAsync();
        Task<bool> UpdateDeviceAsync(DeviceDTO deviceDto);
        Task<bool> DeleteDeviceAsync(int id);
    }
}
