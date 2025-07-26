using Microsoft.EntityFrameworkCore;
using projServer.Data;
using projServer.Entities;
using projServer.Repositories.Interfaces;
using Shared.Enums;
using System.Formats.Asn1;

namespace projServer.Repositories.Implementations
{
    public class RepairRequestRepository:BaseRepository<RepairRequestEntity>, IRepairRequestRepository
    {
        public RepairRequestRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public  async Task<IEnumerable<RepairRequestEntity>> GetAllRequestsWithDeviceAsync()
        {
            return await _dbSet
                .Include(r => r.Device)
                 .ThenInclude(d => d.Room)
                    .Include(r => r.ReportedByUser)

                .ToListAsync();
        }
        public async Task<IEnumerable<RepairRequestEntity>> GetRequestByUserId(int  userId)
        {
            return await _dbSet
                 .Where(r => r.ReportedByUserId == userId)
                .Include(r => r.Device)
                .ThenInclude(d => d.Room)
                .Include(r => r.ReportedByUser)
                .ToListAsync();
        }
        public async Task<IEnumerable<RepairRequestEntity>> GetFixedAndReplacedByMonth(int month, int year)
        {
            return await _dbSet
                .Where(r =>
                    (r.Status == RepairStatus.Fixed || r.Status == RepairStatus.Replaced) &&
                    r.ResolvedDate != null &&
                    r.ResolvedDate.Value.Month == month &&
                    r.ResolvedDate.Value.Year == year)
                .Include(r => r.Device)
                .ThenInclude(d => d.Room)
                .Include(r => r.ReportedByUser)
                .OrderByDescending(r => r.ResolvedDate)
                .ToListAsync();
        }

    }

}

