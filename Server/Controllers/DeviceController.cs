using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using projServer.Services.Interfaces;
using Shared.DTOs;
using AutoMapper;

namespace projServer.Controllers
{
    /// <summary>
    /// handles all device related operations such as
    /// adding, retrieving, updating, and deleting devices.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor for devicecontroller.
        /// injects the device service and automapper.
        /// </summary>
        public DeviceController(IDeviceService deviceService, IMapper mapper)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        /// <summary>
        /// adds a new device to the system
        /// </summary>
        /// <param name="deviceDto">device details (name, type, status, etc.)</param>
        /// <returns>200 ok if success, 400 badrequest if validation fails.</returns>
        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody] DeviceDTO deviceDto)
        {
            if (deviceDto == null)
                return BadRequest("invalid request data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _deviceService.AddDeviceAsync(deviceDto);
            return Ok(new { message = "device added successfully!" });
        }

        /// <summary>
        /// retrieves all devices in the system
        /// </summary>
        /// <returns>list of devices with their details.</returns>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<ActionResult<List<DeviceDTO>>> GetAllDevice()
        {
            var devices = await _deviceService.GetAllDeviceAsync();
            return Ok(devices);
        }

        /// <summary>
        /// updates an existing device record
        /// </summary>
        /// <param name="deviceDto">updated device details.</param>
        /// <returns>
        /// 200 ok if updated,  
        /// 404 notfound if device does not exist.
        /// </returns>
        [HttpPut]
        public async Task<IActionResult> UpdateDevice([FromBody] DeviceDTO deviceDto)
        {
            if (deviceDto == null)
                return BadRequest();

            var success = await _deviceService.UpdateDeviceAsync(deviceDto);
            if (!success)
                return NotFound(new { error = "device not found." });

            return Ok(new { message = "device updated successfully!" });
        }

        /// <summary>
        /// deletes a device from the system.
        /// </summary>
        /// <param name="id">the unique id of the device.</param>
        /// <returns>200 ok if deleted, 400 badrequest if failed.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            await _deviceService.DeleteDeviceAsync(id);
            return Ok(new { message = "device deleted successfully!" });
        }
    }
}
