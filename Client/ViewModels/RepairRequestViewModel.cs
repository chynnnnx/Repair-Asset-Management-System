using Shared.Enums;
using Shared.Helpers;
using System.Text.Json.Serialization;
using Shared.DTOs;

namespace Client.ViewModels
{
    public class RepairRequestViewModel
    {
        public int RepairId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceTag { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime ReportedDate { get; set; } = DateTime.UtcNow;
        public string IssueDescription { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RepairStatus Status { get; set; } = RepairStatus.Pending;

        public DateTime? ResolvedDate { get; set; }
        public string? Remarks { get; set; }
        public int ReportedByUserId { get; set; }
        public string ReportedByUserName { get; set; } = string.Empty;

        // Consistent timezone conversion
        public string ReportedDatePH => TimeHelper.UtcToPH(ReportedDate).ToString("yyyy-MM-dd hh:mm:ss tt");
        public string ResolvedDatePH => ResolvedDate.HasValue
            ? TimeHelper.UtcToPH(ResolvedDate.Value).ToString("yyyy-MM-dd hh:mm:ss tt")
            : "-";
    }

    public static class RepairRequestMappings 
    {
        public static RepairRequestViewModel ToViewModel(this RepairRequestDTO dto) =>
            new RepairRequestViewModel
            {
                RepairId = dto.RepairId,
                DeviceId = dto.DeviceId,
                DeviceTag = dto.DeviceTag,
                RoomName = dto.RoomName,
                ReportedDate = dto.ReportedDate,
                IssueDescription = dto.IssueDescription,
                Status = dto.Status,
                ResolvedDate = dto.ResolvedDate,
                Remarks = dto.Remarks,
                ReportedByUserId = dto.ReportedByUserId,
                ReportedByUserName = dto.ReportedByUserName
            };

        public static RepairRequestDTO ToDTO(this RepairRequestViewModel vm) =>
            new RepairRequestDTO
            {
                RepairId = vm.RepairId,
                DeviceId = vm.DeviceId,
                DeviceTag = vm.DeviceTag,
                RoomName = vm.RoomName,
                ReportedDate = vm.ReportedDate,
                IssueDescription = vm.IssueDescription,
                Status = vm.Status,
                ResolvedDate = vm.ResolvedDate,
                Remarks = vm.Remarks,
                ReportedByUserId = vm.ReportedByUserId,
                ReportedByUserName = vm.ReportedByUserName
            };
    }
}