using Microsoft.EntityFrameworkCore;
using projServer.Data;
using projServer.Entities;
using projServer.Repositories.Interfaces;

namespace projServer.Repositories.Implementations
{
    public class DeviceRepository: BaseRepository<DeviceEntity>, IDeviceRepository
    {
       
        public DeviceRepository(ApplicationDbContext dbContext): base (dbContext) { }

      
        public async Task<List<DeviceEntity>> GetAllDeviceWithRoomAsync()
        {
            return await _dbContext.PC
                .Include(p => p.Room) 
                .ToListAsync();
        }


    }
}
