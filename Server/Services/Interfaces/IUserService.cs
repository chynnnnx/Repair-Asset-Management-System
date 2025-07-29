using Shared.DTOs.Auth;
using projServer.Services.Implementations;
using projServer.Entities;
using Shared.DTOs;
namespace projServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity> RegisterAsync(RegisterUserDTO userDto);
        Task<LoginResultDTO?> LoginAsync(LoginDTO loginDTO);
        Task<bool> AddUserAsync(RegisterUserDTO userDTO);
        Task<bool> UpdateUserInfo(UserDTO userDTO);
        Task<bool> DeleteUser(int userId);
        Task<List<UserDTO>> GetAllUsers();

    }
}
