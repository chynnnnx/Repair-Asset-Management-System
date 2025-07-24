using Shared.Enums;

namespace Shared.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;

        public byte[]? ProfilePicture { get; set; }
    }
}
