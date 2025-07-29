using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projServer.Entities
{
    public class DeviceLogEntity
    {
        [Key]
        public int LogId { get; set; }
        public int? DeviceID { get; set; } 
        public DeviceEntity? Device { get; set; }
        public string? Note { get; set; }
        public int? ActionById { get; set; }
        public UserEntity? ActionBy { get; set; }
        public DateTime DateLogged { get; set; } = DateTime.UtcNow;
    }
}
