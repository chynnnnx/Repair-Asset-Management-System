using projServer.Services.Implementations;
using Shared.DTOs;

namespace projServer.Services.Interfaces
{
    public interface IRepairRequestService
    {
        Task<IEnumerable<RepairRequestDTO>> GetAllRequestsWithDeviceAsync();
        Task<IEnumerable<RepairRequestDTO>> GetRequestByUserId(int userId);
        Task AddRepairRequest(RepairRequestDTO repairRequestDTO);
        Task UpdateRepairRequest(RepairRequestDTO repairRequestDTO);
        Task DeleteRepairRequest(int id);
        Task<IEnumerable<RepairRequestDTO>> GetFixedAndReplacedByMonth(int month, int year);
        Task<byte[]> GenerateReportExcelAsync(int month, int year);
        Task<byte[]> GenerateReportPdfAsync(int month, int year);

    }
}
