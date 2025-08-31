using Shared.DTOs;
using Shared.Helpers;

namespace Client.ViewModels
{
    public class DeviceLogViewModel
    {
        public int LogId { get; set; }

        public int DeviceID { get; set; }

        public string? Note { get; set; }
        public int? ActionById { get; set; }
        public string? ActionByName { get; set; }

        public DateTime DateLogged { get; set; } = DateTime.UtcNow;
        public string DateLoggedPH => TimeHelper.UtcToPH(DateLogged).ToString("yyyy-MM-dd hh:mm:ss tt");
    }

    public static class DeviceLogMappings
    {
        public static DeviceLogViewModel ToViewModel(this DeviceLogDTO dto) =>
            new DeviceLogViewModel
            {
                LogId = dto.LogId,
                DeviceID = dto.DeviceID,
                Note = dto.Note,
                ActionById = dto.ActionById,
                ActionByName = dto.ActionByName,
                DateLogged = dto.DateLogged

            };
        public static DeviceLogDTO ToDTO(this DeviceLogViewModel vm) =>
            new DeviceLogDTO
            {
                LogId = vm.LogId,
                DeviceID = vm.DeviceID,
                Note = vm.Note,
                ActionById = vm.ActionById,
                ActionByName = vm.ActionByName,
                DateLogged = vm.DateLogged

            };
    }
}
