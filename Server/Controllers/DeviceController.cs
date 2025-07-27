using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
    using projServer.Services.Interfaces;
    using Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Security.Claims;

namespace projServer.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]

    public class DeviceController : ControllerBase
        {
            private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;
        public DeviceController(IDeviceService deviceService, IMapper mapper)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody] DeviceDTO deviceDto)
        {
            if (deviceDto == null)
                return BadRequest("Invalid request data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _deviceService.AddDeviceAsync(deviceDto);
                return Ok(new { message = "Device added successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<DeviceDTO>>> GetAllDevice()
        {
            var deviceDtos = await _deviceService.GetAllDeviceAsync(); 
            return Ok(deviceDtos);
        }

    
        [HttpPut]
        public async Task<IActionResult> UpdateDevice ([FromBody] DeviceDTO deviceDto)
        {
            if (deviceDto == null)
                return BadRequest();
            var success = await _deviceService.UpdateDeviceAsync(deviceDto);
            if (!success)
                return NotFound(new { error = "Device not found." });

            return Ok(new { message = "Device updated successfully!" });


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
                await _deviceService.DeleteDevice(id);
                return Ok(new { message = "Device deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
