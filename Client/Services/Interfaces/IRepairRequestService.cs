using Shared.DTOs;

namespace Client.Services.Interfaces
{
    public interface IRepairRequestService
    {
        Task<List<RepairRequestDTO>> GetAllRepairRequestsAsync();
        Task<IEnumerable<RepairRequestDTO>> GetRequestByUserIdAsync(int userId);
        Task<bool> AddRepairRequestAsync(RepairRequestDTO repairRequestDto);
        Task<bool> UpdateRepairRequestAsync(RepairRequestDTO repairRequestDto);
        Task<bool> DeleteRepairRequestAsync(int id);
        Task<List<RepairRequestDTO>> GetSummaryByMonthAsync(int month, int year);
        Task<byte[]> DownloadPdfReportAsync(int month, int year);
        Task<byte[]> DownloadExcelReportAsync(int month, int year);
    }
}
