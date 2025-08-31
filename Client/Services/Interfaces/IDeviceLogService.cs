using Client.ViewModels;

namespace Client.Services.Interfaces
{
    public interface IDeviceLogService
    {
        Task<List<DeviceLogViewModel>> GetAllLogs();
    }
}
