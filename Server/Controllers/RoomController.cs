using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projServer.Services.Interfaces;
using Shared.DTOs;

namespace projServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
                return BadRequest("Room data is required.");

            var success = await _roomService.AddRoomAsync(roomDto);
            if (success)
                return Ok(new { message = "Room added successfully" });

            return StatusCode(500, new { message = "Failed to add room." });
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<List<RoomDTO>>> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound(new { message = $"Room with ID {id} not found." });

            return Ok(room);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
                return BadRequest("Room data is required.");

            var success = await _roomService.UpdateRoomAsync(roomDto);
            if (success)
                return Ok(new { message = "updated successfully" });

            return NotFound(new { message = " Update failed." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var success = await _roomService.DeleteRoomAsync(id);
            if (success)
                return Ok(new { message = "Room deleted successfully" });

            return NotFound(new { message = $"Room with ID {id} not found or delete failed." });
        }
    }

}
