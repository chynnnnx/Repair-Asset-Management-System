using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projServer.Services.Interfaces;
using Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Formats.Asn1;

namespace projServer.Controllers
{  
    /// <summary>
    /// handles all operations related to device logs such as
    /// retrieving logs by device id or getting all logs.
    /// </summary>
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class DeviceLogController : ControllerBase
    {
        private readonly IDeviceLogService _logService;
        private readonly IMapper _mapper;
        
         /// <summary>
        /// constructor for devicelogcontroller
        /// injects the log service and automapper.
        /// </summary>
        public DeviceLogController(IDeviceLogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        
        }
          /// <summary>
        /// retrieves all logs for a specific device.
        /// </summary>
        /// <param name="deviceId">the unique id of the device</param>
        /// <returns>list of logs for the given device.</returns>
        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetLogsByDeviceId(int deviceId)
        {
            var logs = await _logService.GetLogs(deviceId);
            return Ok(logs);
        }
        
        /// <summary>
        /// retrieves all device logs in the system
        /// </summary>
        /// <returns>list of all device logs.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _logService.GetAllLogs();
            return Ok(logs);
        }
    }
}

