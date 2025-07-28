using Shared.DTOs.Auth;
using Shared.DTOs;
namespace Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDTO?> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync();
        Task<string?> RegisterAccount(RegisterUserDTO registerDto);
        Task<List<UserDTO>> GetAllUsers();
        Task<bool> AddUserAsync(RegisterUserDTO userDto);
        Task<bool> UpdateUserInfo(UserDTO userDto);
            Task<bool> DeleteUser(int userId);
    

    }
}
