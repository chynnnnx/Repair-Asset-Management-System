using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using projServer.Entities;
using projServer.Helpers;
using projServer.Repositories.Interfaces;
using projServer.Services.Interfaces;
using Shared.DTOs;
using System.Security.Claims;

namespace projServer.Services.Implementations
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _device;
        private readonly ILogger<DeviceService> _logger;
        private readonly IMapper _mapper;
        private readonly IDeviceLogRepository _log;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeviceService(
            IDeviceRepository device,
            IMapper mapper,
            ILogger<DeviceService> logger,
            IDeviceLogRepository log,
            IHttpContextAccessor httpContextAccessor)
        {
            _device = device;
            _mapper = mapper;
            _logger = logger;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
        }

        private (int userId, string fullName) GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nameClaim = user?.FindFirst(ClaimTypes.Name)?.Value;

            int.TryParse(userIdClaim, out var userId);
            return (userId, nameClaim ?? "Unknown");
        }

        public async Task AddDeviceAsync(DeviceDTO deviceDto)
        {
            try
            {
                var deviceEntity = _mapper.Map<DeviceEntity>(deviceDto);
                await _device.AddAsync(deviceEntity);

                var (userId, fullName) = GetCurrentUser();
                var log = DeviceLogHelper.Create(deviceEntity.DeviceID, userId, "Add", fullName);

                await _log.AddAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add Device ({deviceDto.Tag}): {ex.GetBaseException().Message}");
                throw;
            }
        }

        public async Task<List<DeviceDTO>> GetAllDeviceAsync()
        {
            try
            {
                var deviceEntities = await _device.GetAllDeviceWithRoomAsync();
                return _mapper.Map<List<DeviceDTO>>(deviceEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get all Devices: {ex.GetBaseException().Message}");
                throw;
            }
        }

      
        public async Task<bool> UpdateDeviceAsync(DeviceDTO deviceDTO)
        {
            try
            {
                var existing = await _device.GetByIdAsync(deviceDTO.DeviceID);
                if (existing == null) return false;

                var (userId, fullName) = GetCurrentUser();

                var log = DeviceLogHelper.CreateFromChanges(existing, deviceDTO, userId, fullName);
                _mapper.Map(deviceDTO, existing);
                await _device.UpdateAsync(existing);
                await _log.AddAsync(log);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update Devices ({deviceDTO.Tag}): {ex.GetBaseException().Message}");
                return false;
            }
        }


        public async Task DeleteDevice(int id)
        {
            try
            {
                var existing = await _device.GetByIdAsync(id);
                if (existing == null)
                    throw new Exception($"Device with ID {id} not found.");

                var (userId, fullName) = GetCurrentUser();
                var log = DeviceLogHelper.CreateDeleted(existing.DeviceID, userId, fullName);
                await _log.AddAsync(log);

                await _device.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete Device (ID: {id}): {ex.GetBaseException().Message}");
                throw;
            }
        }



    }
}
