namespace Shared.DTOs.Auth
{
    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
      
    }
}

