using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
using Shared.Enums;
    namespace Shared.DTOs.Auth
    {
        public class RegisterUserDTO
        {
            public string LastName { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string MiddleName { get; set; } = string.Empty;
            public string Gender { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public UserRole Role { get; set; } = UserRole.User;
       
        }
    }
