using projServer.Entities;

namespace projServer.Repositories.Interfaces
{
    public interface IDeviceLogRepository : IBaseRepository<DeviceLogEntity>
    {
        Task<IEnumerable<DeviceLogEntity>> GetLogsByDeviceId(int deviceId);
        Task<IEnumerable<DeviceLogEntity>> GetAllLogsWithUserAsync();
    }
}
