using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace projServer.Entities
{
    public class RepairRequestEntity
    {
        [Key]
        public int RepairId { get; set; }
        public int DeviceId { get; set; }
        public DeviceEntity? Device { get; set; }
        public DateTime ReportedDate { get; set; } = DateTime.UtcNow;
        public string IssueDescription { get; set; } = string.Empty;
        public RepairStatus Status { get; set; } = RepairStatus.Pending;
        public DateTime? ResolvedDate { get; set; } = DateTime.UtcNow;
        public string? Remarks { get; set; }
        public int ReportedByUserId{ get; set; }
        public  UserEntity? ReportedByUser { get; set; }
    }
}
