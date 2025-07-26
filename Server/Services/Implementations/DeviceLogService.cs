using AutoMapper;
using projServer.Repositories.Interfaces;
using Shared.DTOs;
using projServer.Services.Interfaces;
using projServer.Entities;
using Microsoft.Extensions.Logging;

namespace projServer.Services.Implementations
{
    public class DeviceLogService : IDeviceLogService
    {
        private readonly IDeviceLogRepository _log;
        private readonly ILogger<DeviceLogService> _logger;
        private readonly IMapper _mapper;

        public DeviceLogService(IDeviceLogRepository log, IMapper mapper, ILogger<DeviceLogService> logger)
        {
            _log = log;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<DeviceLogDTO>> GetLogs(int deviceId)
        {
            try
            {
                var logs = await _log.GetLogsByDeviceId(deviceId);
                return _mapper.Map<IEnumerable<DeviceLogDTO>>(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving logs for device ({deviceId}): {ex.GetBaseException().Message}");
                return Enumerable.Empty<DeviceLogDTO>();
            }
        }

        public async Task<IEnumerable<DeviceLogDTO>> GetAllLogs()
        {
            try
            {
                var logs = await _log.GetAllLogsWithUserAsync();
                return _mapper.Map<IEnumerable<DeviceLogDTO>>(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving all device logs: {ex.GetBaseException().Message}");
                return Enumerable.Empty<DeviceLogDTO>();
            }
        }

     
    }
}
