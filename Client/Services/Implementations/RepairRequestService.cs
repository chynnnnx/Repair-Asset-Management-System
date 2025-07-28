using Client.Services.Interfaces;
using Shared.DTOs;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;

namespace Client.Services.Implementations
{
    public class RepairRequestService: BaseHttpService,  IRepairRequestService
    {
        public RepairRequestService(HttpClient http, ILocalStorageService localStorage, ILogger<RepairRequestService> logger)
            : base(http, localStorage, logger) { }
        public async Task<List<RepairRequestDTO>> GetAllRepairRequestsAsync()
        {
            return await GetAsync<List<RepairRequestDTO>>("api/RepairRequest") ?? new();
        }
        public async Task<IEnumerable<RepairRequestDTO>> GetRequestByUserIdAsync(int userId)
        {
            return await GetAsync<IEnumerable<RepairRequestDTO>>($"api/RepairRequest/{userId}") ?? new List<RepairRequestDTO>();
        }
        public async Task<bool> AddRepairRequestAsync(RepairRequestDTO repairRequestDto)
        {
            return await PostAsync("api/RepairRequest", repairRequestDto);
        }
        
        public async Task<bool> UpdateRepairRequestAsync(RepairRequestDTO repairRequestDto)
        {
            return await PutAsync("api/RepairRequest", repairRequestDto);
        }
        public async Task<bool> DeleteRepairRequestAsync(int id)
        {
            return await DeleteAsync($"api/RepairRequest/{id}");
        }
        public async Task<List<RepairRequestDTO>> GetSummaryByMonthAsync(int month, int year)
        {
            return await GetAsync<List<RepairRequestDTO>>(
                $"api/RepairRequest/summary?month={month}&year={year}"
            ) ?? new();
        }
        public async Task<byte[]> DownloadPdfReportAsync(int month, int year)
        {
            return await GetFileAsync($"api/RepairRequest/export/pdf?month={month}&year={year}");
        }

        public async Task<byte[]> DownloadExcelReportAsync(int month, int year)
        {
            return await GetFileAsync($"api/RepairRequest/export/excel?month={month}&year={year}");
        }


    }
}
