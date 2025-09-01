using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projServer.Services.Interfaces;
using Shared.DTOs;

namespace projServer.Controllers
{
    /// <summary>
    /// handles room operations like add, get, update, and delete
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        /// <summary>
        /// constructor for roomcontroller
        /// </summary>
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// add a new room
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
                return BadRequest("room data is required.");

            var success = await _roomService.AddRoomAsync(roomDto);
            if (success)
                return Ok(new { message = "room added successfully" });

            return StatusCode(500, new { message = "failed to add room." });
        }

        /// <summary>
        /// get all rooms
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<List<RoomDTO>>> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        /// <summary>
        /// get a room by id
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound(new { message = $"room with id {id} not found." });

            return Ok(room);
        }

        /// <summary>
        /// update an existing room
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
                return BadRequest("room data is required.");

            var success = await _roomService.UpdateRoomAsync(roomDto);
            if (success)
                return Ok(new { message = "updated successfully" });

            return NotFound(new { message = "update failed." });
        }

        /// <summary>
        /// delete a room by id
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var success = await _roomService.DeleteRoomAsync(id);
            if (success)
                return Ok(new { message = "room deleted successfully" });

            return NotFound(new { message = $"room with id {id} not found or delete failed." });
        }
    }
}
