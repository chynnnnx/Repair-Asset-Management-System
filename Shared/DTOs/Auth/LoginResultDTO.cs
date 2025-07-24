using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Auth
{
    public class LoginResultDTO
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
    }
}
