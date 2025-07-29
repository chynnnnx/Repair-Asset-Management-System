using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Auth;
using projServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Shared.DTOs;

namespace projServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _authService;
        public AuthController(IUserService authService) 
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async  Task<IActionResult> RegisterAsync([FromBody] RegisterUserDTO userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.PasswordHash))
            {
                return BadRequest("Invalid user data.");
            }
            var result = await _authService.RegisterAsync(userDto);
            if (result == null)
            {
                return Conflict("User already exists.");
            }
            return Ok(result);
        }

 
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.PasswordHash))
            {
                return BadRequest("Invalid login data.");
            }

            var result = await _authService.LoginAsync(loginDTO); 
            if (result == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(result); 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult<UserDTO>> GetAllUsers()
        {
            var user = await _authService.GetAllUsers();
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUsers([FromBody] RegisterUserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest("Invalid user data.");

            bool success = await _authService.AddUserAsync(userDTO);
            if (!success)
                return Conflict("User already exists or failed to add user.");

            return Ok(new { message = "User added successfully!" });
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest();

            bool updated = await _authService.UpdateUserInfo(userDTO);
            if (!updated)
                return NotFound("User not found or update failed.");

            return Ok(new { message = "User information updated successfully!" });

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user ID.");

            bool deleted = await _authService.DeleteUser(userId);
            if (!deleted)
                return NotFound("User not found or delete failed.");

            return Ok(new { message = "User deleted successfully!" });
        }

    }
}
