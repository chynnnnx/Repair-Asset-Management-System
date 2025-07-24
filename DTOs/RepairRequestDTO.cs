    using Shared.Enums;
    using Shared.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    namespace Shared.DTOs
    {
        public class RepairRequestDTO
        {
            public int RepairId { get; set; }
            public int DeviceId { get; set; }
            public string DeviceTag { get; set; } = string.Empty;
            public string RoomName { get; set; } = string.Empty;
            public DateTime ReportedDate { get; set; } = DateTime.UtcNow;
            public string IssueDescription { get; set; } = string.Empty;
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public RepairStatus Status { get; set; } = RepairStatus.Pending;
            public DateTime? ResolvedDate { get; set; } 
            public string? Remarks { get; set; }
            public int ReportedByUserId { get; set; }
            public string ReportedByUserName { get; set; } = string.Empty;
            public string ReportedDatePH => TimeHelper.UtcToPH(ReportedDate).ToString("yyyy-MM-dd hh:mm:ss tt");

            public string ResolvedDatePH => ResolvedDate.HasValue
                ? TimeHelper.LocalToPH(ResolvedDate.Value).ToString("yyyy-MM-dd hh:mm:ss tt")
                : "-";
     
    }
    }
