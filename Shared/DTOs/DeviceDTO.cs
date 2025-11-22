using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class DeviceDTO
    {
        public int DeviceID { get; set; }
        public string Tag { get; set; } = string.Empty;
          public string Type { get; set; } = string.Empty;
        public DeviceStatus Status { get; set; } = DeviceStatus.Offline;
         public int RoomId { get; set; }
        public string RoomName { get; set; } = "N/A";

    }
}

