using Client.ViewModels;

namespace Client.Services.Interfaces
{
    public interface IRepairRequestService
    {
        Task<List<RepairRequestViewModel>> GetAllRepairRequestsAsync();
        Task<List<RepairRequestViewModel>> GetRequestByUserIdAsync(int userId);
        Task<bool> AddRepairRequestAsync(RepairRequestViewModel repairRequest);
        Task<bool> UpdateRepairRequestAsync(RepairRequestViewModel repairRequest);
        Task<bool> DeleteRepairRequestAsync(int id);
        Task<List<RepairRequestViewModel>> GetSummaryByMonthAsync(int month, int year);
        Task<byte[]> DownloadPdfReportAsync(int month, int year);
        Task<byte[]> DownloadExcelReportAsync(int month, int year);
    }
}
