using System.ComponentModel.DataAnnotations;

namespace projServer.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public ICollection<RepairRequestEntity> RepairRequestsReported { get; set; } = new List<RepairRequestEntity>();


    }
}
