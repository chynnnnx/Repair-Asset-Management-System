using Shared.DTOs.Auth;
using Client.ViewModels;
namespace Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDTO?> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync();
        Task<string?> RegisterAccount(RegisterUserDTO registerDto);
        Task<List<UserViewModel>> GetAllUsers();
        Task<bool> AddUserAsync(UserViewModel userVm);
        Task<bool> UpdateUserInfo(UserViewModel userVm);
        Task<bool> DeleteUser(int userId);
    

    }
}
