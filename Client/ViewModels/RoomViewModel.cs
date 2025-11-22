using Shared.DTOs;

namespace Client.ViewModels
{
    
    public class RoomViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    public static class RoomMappings
    {
        public static RoomViewModel ToViewModel(this RoomDTO dto) =>
            new RoomViewModel
            {
                RoomId = dto.RoomId,
                RoomName = dto.RoomName,
                Location = dto.Location,   
                Capacity = dto.Capacity    
            };

        public static RoomDTO ToDTO(this RoomViewModel vm) =>
            new RoomDTO
            {
                RoomId = vm.RoomId,
                RoomName = vm.RoomName,
                Location = vm.Location,   
                Capacity = vm.Capacity    
            };
    }


}
