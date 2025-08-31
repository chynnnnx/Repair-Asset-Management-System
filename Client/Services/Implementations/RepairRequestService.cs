using Client.Services.Interfaces;
using Shared.DTOs;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using Client.ViewModels;

namespace Client.Services.Implementations
{
    public class RepairRequestService: BaseHttpService,  IRepairRequestService
    {
        public RepairRequestService(HttpClient http, ILocalStorageService localStorage, ILogger<RepairRequestService> logger)
            : base(http, localStorage, logger) { }
        public async Task<List<RepairRequestViewModel>> GetAllRepairRequestsAsync()
        {
            var dtos =await GetAsync<List<RepairRequestDTO>>("api/RepairRequest") ?? new();
            return dtos.Select(x => x.ToViewModel()).ToList();

        }
        public async Task<List<RepairRequestViewModel>> GetRequestByUserIdAsync(int userId)
        {
            var dtos = await GetAsync<IEnumerable<RepairRequestDTO>>($"api/RepairRequest/{userId}") ?? Enumerable.Empty<RepairRequestDTO>();
            return dtos.Select(x => x.ToViewModel()).ToList();
        }

        public async Task<bool> AddRepairRequestAsync(RepairRequestViewModel repairRequest)
        {
            return await PostAsync("api/RepairRequest", repairRequest.ToDTO());

        }

        public async Task<bool> UpdateRepairRequestAsync(RepairRequestViewModel repairRequest)
        {
            return await PutAsync("api/RepairRequest", repairRequest.ToDTO());
        }
        public async Task<bool> DeleteRepairRequestAsync(int id)
        {
            return await DeleteAsync($"api/RepairRequest/{id}");
        }
        public async Task<List<RepairRequestViewModel>> GetSummaryByMonthAsync(int month, int year)
        {
            var dtos = await GetAsync<List<RepairRequestDTO>>($"api/RepairRequest/summary?month={month}&year={year}")
                       ?? new List<RepairRequestDTO>();
            return dtos.Select(x => x.ToViewModel()).ToList();
        }

        public async Task<byte[]> DownloadPdfReportAsync(int month, int year)
        {
            var fileBytes = await GetFileAsync($"api/RepairRequest/export/pdf?month={month}&year={year}");
            return fileBytes ?? Array.Empty<byte>();
        }

        public async Task<byte[]> DownloadExcelReportAsync(int month, int year)
        {
            var fileBytes = await GetFileAsync($"api/RepairRequest/export/excel?month={month}&year={year}");
            return fileBytes ?? Array.Empty<byte>();
        }



    }
}
