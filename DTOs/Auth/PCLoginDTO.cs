using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Auth
{
    
        public class PCLoginDTO
        {
            public int UserId { get; set; }
            public string Tag { get; set; } = string.Empty;
            public int RoomId { get; set; }
        }
    }


