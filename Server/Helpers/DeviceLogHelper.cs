using projServer.Entities;
using Shared.DTOs;

namespace projServer.Helpers
{
    public static class DeviceLogHelper
    {
        public static DeviceLogEntity Create(int deviceId, int userId, string action, string? fullName = null, string? customNote = null)
        {
            return new DeviceLogEntity
            {
                DeviceID = deviceId,
                ActionById = userId,
                Note = customNote ?? GenerateNote(action, fullName),
                DateLogged = DateTime.UtcNow
            };
        }

        public static DeviceLogEntity CreateFromChanges(DeviceEntity oldData, DeviceDTO newData, int userId, string fullName)
        {
            var changes = new List<string>();

            if (oldData.Status != newData.Status)
                changes.Add($"Status: {oldData.Status} to {newData.Status}");

            if (oldData.RoomId != newData.RoomId)
                changes.Add($"Room: {oldData.RoomId} to {newData.RoomId}");

            if (oldData.Tag != newData.Tag)
               changes.Add($"Tag: {oldData.Tag} to {newData.Tag}");

            var note = changes.Count > 0
                ? $"{string.Join(", ", changes)} "
                : $"Updated device, no changes detected";

            return Create(newData.DeviceID, userId, "Update", fullName, note);
        }

        private static string GenerateNote(string action, string? fullName = null) => action switch
        {
            "Add" => $"Device added",
            "Update" => $"Device updated",
            "Delete" => $"Device deleted",
            _ => $"Performed an action."
        };
        public static DeviceLogEntity CreateDeleted(int deviceId, int userId, string? fullName = null)
        {
            return new DeviceLogEntity
            {
                DeviceID = deviceId,
                ActionById = userId,
                Note = $"Device #{deviceId} was deleted.",
                DateLogged = DateTime.UtcNow
            };
        }

    
    }
}
