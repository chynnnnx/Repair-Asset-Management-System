using Client.ViewModels;

namespace Client.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<List<DeviceViewModel>> GetAllDevicesAsync();
        Task<bool> AddDeviceAsync(DeviceViewModel device);
        Task<bool> UpdateDeviceAsync(DeviceViewModel device);
            Task<bool> DeleteDeviceAsync(int deviceId);
    }
}
