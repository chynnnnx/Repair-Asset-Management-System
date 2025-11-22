using System.ComponentModel.DataAnnotations;

namespace projServer.Entities
{
    public class RoomEntity
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public ICollection<DeviceEntity> Devices { get; set; } = new List<DeviceEntity>();
    }
}
