using System.ComponentModel.DataAnnotations;

namespace projServer.Entities
{
    public class RoomEntity
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public ICollection<DeviceEntity> PCs { get; set; } = new List<DeviceEntity>();
    
    }
}
