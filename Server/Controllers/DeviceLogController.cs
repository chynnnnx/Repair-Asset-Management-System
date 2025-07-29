using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projServer.Services.Interfaces;
using Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Formats.Asn1;

namespace projServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class DeviceLogController : ControllerBase
    {
        private readonly IDeviceLogService _logService;
        private readonly IMapper _mapper;

        public DeviceLogController(IDeviceLogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }
        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetLogsByDeviceId(int deviceId)
        {
            var logs = await _logService.GetLogs(deviceId);
            return Ok(logs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _logService.GetAllLogs();
            return Ok(logs);
        }
    }
}
