using Shared.DTOs;
using Shared.Enums;

namespace Client.ViewModels
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;
    }

    public static class UserMappings
    {
        public static UserViewModel ToViewModel(this UserDTO dto) =>
            new UserViewModel
            {
                UserID = dto.UserID,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Gender = dto.Gender,
                Email = dto.Email,
                Role = dto.Role

            };
        public static UserDTO ToDTO(this UserViewModel vm) =>
            new UserDTO
            {
                UserID = vm.UserID,
                LastName = vm.LastName,
                FirstName = vm.FirstName,
                MiddleName = vm.MiddleName,
                Gender = vm.Gender,
                Email = vm.Email,
                Role = vm.Role
            };
    }
}
