using Shared.DTOs;

namespace Client.ViewModels
{
    
    public class RoomViewModel
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
    }

    public static class RoomMappings
    {
        public static RoomViewModel ToViewModel(this RoomDTO dto) =>
            new RoomViewModel
            {
                RoomId = dto.RoomId,
                RoomName = dto.RoomName
            };

        public static RoomDTO ToDTO(this RoomViewModel vm) =>
            new RoomDTO
            {
                RoomId = vm.RoomId,
                RoomName = vm.RoomName
            };
    }

}
