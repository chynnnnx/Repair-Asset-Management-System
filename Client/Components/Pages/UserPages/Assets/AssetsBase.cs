using Microsoft.AspNetCore.Components;
using Client.Services.Interfaces;
using Client.ViewModels; 
using Shared.Enums;
using MudBlazor;
using Shared.DTOs;
using System.Linq;

namespace Client.Components.Pages.UserPages.Assets
{
    public class AssetsBase : ComponentBase
    {
        [Inject] protected IDeviceService DeviceService { get; set; }
        [Inject] protected IRoomService RoomService { get; set; }

        public List<DeviceViewModel> device = new();   
        public List<RoomViewModel> rooms = new();

        protected override async Task OnInitializedAsync()
        {
            device = await DeviceService.GetAllDevicesAsync();
            rooms = await RoomService.GetAllRoomsAsync(); 
        }
        protected string GetStatusLabel(DeviceStatus status) => status switch
        {
            DeviceStatus.Online => "Online",
            DeviceStatus.Offline => "Offline",
            DeviceStatus.NotResponding => "Not Responding",
            DeviceStatus.Maintenance => "Maintenance",
            _ => "Unknown"
        };

        protected Color GetStatusColor(DeviceStatus status) => status switch
        {
            DeviceStatus.Online => Color.Success,
            DeviceStatus.Offline => Color.Info,
            DeviceStatus.NotResponding => Color.Error,
            DeviceStatus.Maintenance => Color.Warning,
            _ => Color.Default
        };

        protected string GetStatusIcon(DeviceStatus status) => status switch
        {
            DeviceStatus.Online => Icons.Material.Filled.CheckCircle,
            DeviceStatus.Offline => Icons.Material.Filled.PowerOff,
            DeviceStatus.NotResponding => Icons.Material.Filled.Error,
            DeviceStatus.Maintenance => Icons.Material.Filled.Build,
            _ => Icons.Material.Filled.Help
        };
    }
}
