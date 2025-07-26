using AutoMapper;
using Microsoft.Extensions.Logging;
using projServer.Entities;
using projServer.Repositories.Interfaces;
using projServer.Services.Interfaces;
using Shared.DTOs;
using projServer.Helpers;
using System.Globalization;

namespace projServer.Services.Implementations
{
    public class RepairRequestService : IRepairRequestService
    {
        private readonly ILogger<RepairRequestService> _logger;
        private readonly IRepairRequestRepository _repairRequestRepo;
        private readonly IMapper _mapper;

        public RepairRequestService(
            IRepairRequestRepository repairRequestRepo,
            IMapper mapper,
            ILogger<RepairRequestService> logger)
        {
            _repairRequestRepo = repairRequestRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RepairRequestDTO>> GetAllRequestsWithDeviceAsync()
        {
            try
            {
                var entities = await _repairRequestRepo.GetAllRequestsWithDeviceAsync();
                return _mapper.Map<IEnumerable<RepairRequestDTO>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all repair requests with devices");
                throw;
            }
        }

        public async Task<IEnumerable<RepairRequestDTO>> GetRequestByUserId(int userId)
        {
            try
            {
                var entities = await _repairRequestRepo.GetRequestByUserId(userId);
                return _mapper.Map<IEnumerable<RepairRequestDTO>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving repair requests for user ID {userId}");
                throw;
            }
        }

        public   async Task AddRepairRequest(RepairRequestDTO repairRequestDTO)
        {
            try
            {
                var reqEntity = _mapper.Map<RepairRequestEntity>(repairRequestDTO);
                await  _repairRequestRepo.AddAsync(reqEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add Repair Request: {ex.GetBaseException().Message}");
                throw;
            }
        }

        public  async Task  UpdateRepairRequest(RepairRequestDTO repairRequestDTO)
        {
            try
            {
                var reqEntity = _mapper.Map<RepairRequestEntity>(repairRequestDTO);
                await _repairRequestRepo.UpdateAsync(reqEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update Repair Request: {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task DeleteRepairRequest(int id)
        {
            try
            {
                var existing = await _repairRequestRepo.GetByIdAsync(id);
                if (existing == null)
                    throw new KeyNotFoundException($"Repair Request with ID {id} not found.");

                await _repairRequestRepo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete Repair Request with ID {id}: {ex.GetBaseException().Message}");
                throw;
            }
        }
        public async Task<IEnumerable<RepairRequestDTO>> GetFixedAndReplacedByMonth(int month, int year)
        {
            try
            {
                var entities = await _repairRequestRepo.GetFixedAndReplacedByMonth(month, year);
                return _mapper.Map<IEnumerable<RepairRequestDTO>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get reports for {month}/{year}: {ex.GetBaseException().Message}");
                throw;
            }
        }

        public async Task<byte[]> GenerateReportExcelAsync(int month, int year)
        {
            try
            {
                var reports = await GetFixedAndReplacedByMonth(month, year);
                return ExcelReportBuilder.Build(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to generate Excel report for {month}/{year}: {ex.GetBaseException().Message}");
                throw;
            }
        }

        public async Task<byte[]> GenerateReportPdfAsync(int month, int year)
        {
            try
            {
                var reports = await GetFixedAndReplacedByMonth(month, year);
                var title = $"Repair Report Summary - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}";
                return PdfReportBuilder.Build(reports, title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to generate PDF report for {month}/{year}: {ex.GetBaseException().Message}");
                throw;
            }
        }

    }
}
