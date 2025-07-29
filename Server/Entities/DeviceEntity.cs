using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace projServer.Entities
{
    public class DeviceEntity
    {
        [Key]
        public int DeviceID { get; set; }
        public string Tag { get; set; } = string.Empty;
        public DeviceStatus Status { get; set; } = DeviceStatus.Online;
        public int RoomId { get; set; }
        public RoomEntity? Room { get; set; }
        public ICollection<RepairRequestEntity>? RepairRequests { get; set; }

    }
}
