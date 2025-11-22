using Shared.DTOs;
using Shared.Enums;


namespace Client.ViewModels
{
    public class DeviceViewModel
    {
        public int DeviceID { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DeviceStatus Status { get; set; } = DeviceStatus.Offline;
        public int RoomId { get; set; }
        public string RoomName { get; set; } = "N/A";
    }

   public static class  DeviceMappings
    {
        public static DeviceViewModel ToViewModel(this DeviceDTO dto) =>
            new DeviceViewModel
            {
                DeviceID = dto.DeviceID,
                Tag = dto.Tag,
                Type = dto.Type,
                Status = dto.Status,
                RoomId = dto.RoomId,
                RoomName = dto.RoomName
            };

        public static DeviceDTO ToDTO(this DeviceViewModel vm) =>
            new DeviceDTO
            {
                DeviceID = vm.DeviceID,
                Tag = vm.Tag,
                Type = vm.Type,
                Status = vm.Status,
                RoomId = vm.RoomId,
                RoomName = vm.RoomName
            };
    }
}


