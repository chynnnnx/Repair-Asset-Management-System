using Microsoft.EntityFrameworkCore;
using projServer.Data;
using projServer.Entities;
using projServer.Repositories.Interfaces;

namespace projServer.Repositories.Implementations
{
    public class DeviceLogRepository: BaseRepository<DeviceLogEntity>, IDeviceLogRepository
    {
        public DeviceLogRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<DeviceLogEntity>> GetLogsByDeviceId(int deviceId)
        {
            return await _dbContext.DeviceLogs
                .Where(log => log.DeviceID == deviceId)
                .Include(log => log.ActionBy) 
                .OrderByDescending(log => log.DateLogged)
                .ToListAsync();
        }
        public async Task<IEnumerable<DeviceLogEntity>> GetAllLogsWithUserAsync()
        {
            return await _dbSet
                .Include(x => x.ActionBy)
                .ToListAsync();
        }

    }
}
