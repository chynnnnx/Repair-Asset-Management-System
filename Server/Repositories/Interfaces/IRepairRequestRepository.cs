using projServer.Entities;

namespace projServer.Repositories.Interfaces
{
    public interface IRepairRequestRepository: IBaseRepository<RepairRequestEntity>
    {
        Task<IEnumerable<RepairRequestEntity>> GetAllRequestsWithDeviceAsync();
        Task<IEnumerable<RepairRequestEntity>> GetRequestByUserId(int userId);
        Task<IEnumerable<RepairRequestEntity>> GetFixedAndReplacedByMonth(int month, int year);
    }
}
