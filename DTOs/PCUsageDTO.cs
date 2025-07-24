using Shared.Helpers;

public class PCUsageDTO
{
    public int PCUsageId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public int UserID { get; set; }
    public string FullName { get; set; } = string.Empty;

    public DateTime DateTimeIn { get; set; } = DateTime.UtcNow;
    public DateTime? DateTimeOut { get; set; } = null;

    public int RoomId { get; set; }
    public string RoomName { get; set; } = "N/A";

    // Now this will work correctly - no double conversion
   // public string DateTimeInPH => TimeHelper.ToPHTime(DateTimeIn);
    //public string? DateTimeOutPH => DateTimeOut.HasValue ? TimeHelper.ToPHTime(DateTimeOut.Value) : null;

    public string Duration => TimeHelper.FormatDuration(DateTimeIn, DateTimeOut);
}