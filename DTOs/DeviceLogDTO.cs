using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class DeviceLogDTO
    {
        public int LogId { get; set; }

        public int DeviceID { get; set; }

        public string? Note { get; set; }
        public int? ActionById { get; set; }
        public string? ActionByName { get; set; }

        public DateTime DateLogged { get; set; } = DateTime.UtcNow;
        public string DateLoggedPH => TimeHelper.UtcToPH(DateLogged).ToString("yyyy-MM-dd hh:mm:ss tt");
    }
}
