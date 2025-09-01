using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Auth;
using projServer.Services.Interfaces;
using Shared.DTOs;

namespace projServer.Controllers
{
    /// <summary>
    /// handles authentication and user management operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] 
    public class AuthController : ControllerBase
    {
        private readonly IUserService _authService;

        /// <summary>
        /// constructor for authcontroller
        /// </summary>
        public AuthController(IUserService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// register a new user (anyone)
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDTO userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.PasswordHash))
                return BadRequest("invalid user data.");

            var result = await _authService.RegisterAsync(userDto);
            if (result == null)
                return Conflict("user already exists.");

            return Ok(result);
        }

        /// <summary>
        /// login user (anyone)
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.PasswordHash))
                return BadRequest("invalid login data.");

            var result = await _authService.LoginAsync(loginDTO);
            if (result == null)
                return Unauthorized("invalid email or password.");

            return Ok(result);
        }

        /// <summary>
        /// get all users (admin only)
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<UserDTO>> GetAllUsers()
        {
            var users = await _authService.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// add a new user (admin only)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddUsers([FromBody] RegisterUserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest("invalid user data.");

            bool success = await _authService.AddUserAsync(userDTO);
            if (!success)
                return Conflict("user already exists or failed to add user.");

            return Ok(new { message = "user added successfully" });
        }

        /// <summary>
        /// update user information (admin or user)
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest();

            bool updated = await _authService.UpdateUserInfo(userDTO);
            if (!updated)
                return NotFound("user not found or update failed.");

            return Ok(new { message = "user information updated successfully" });
        }

        /// <summary>
        /// delete a user by id (admin only)
        /// </summary>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("invalid user id.");

            bool deleted = await _authService.DeleteUser(userId);
            if (!deleted)
                return NotFound("user not found or delete failed.");

            return Ok(new { message = "user deleted successfully" });
        }
    }
}
