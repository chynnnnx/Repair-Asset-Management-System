using projServer.Services.Implementations;
using Shared.DTOs;

namespace projServer.Services.Interfaces
{
    public interface IRepairRequestService
    {
        Task<IEnumerable<RepairRequestDTO>> GetAllRequestsWithDeviceAsync();
        Task<IEnumerable<RepairRequestDTO>> GetRequestByUserId(int userId);
        Task<bool> UpdateRepairRequest(RepairRequestDTO repairRequestDTO);
        Task<bool> AddRepairRequest(RepairRequestDTO repairRequestDTO);   
        Task<bool> DeleteRepairRequest(int id);
        Task<IEnumerable<RepairRequestDTO>> GetFixedAndReplacedByMonth(int month, int year);
        Task<byte[]> GenerateReportExcelAsync(int month, int year);
        Task<byte[]> GenerateReportPdfAsync(int month, int year);

    }
}
