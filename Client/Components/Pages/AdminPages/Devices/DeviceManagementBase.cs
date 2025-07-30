using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs;
using Client.Services.Interfaces;
using Client.Components.Dialogs.Devices;
using Shared.Enums;
using Client.Components.Pages.Common;

namespace Client.Components.Pages.AdminPages.Devices
{
    public class DeviceManagementBase : ComponentBase
    {
        [Inject] protected IDeviceService DeviceService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IRoomService RoomService { get; set; } = default!;

        protected List<RoomDTO> rooms = new();
        protected List<DeviceDTO> devices = new();
        protected HashSet<DeviceDTO> selectedDevices = new();

        protected DeviceDTO newDevice = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadDevices();
            await LoadRooms();
        }

        protected async Task LoadRooms()
        {
            rooms = await RoomService.GetAllRoomsAsync();
        }

        protected async Task LoadDevices()
        {
            devices = await DeviceService.GetAllDevicesAsync();
        }

        protected async Task AddDevice()
        {
            if (string.IsNullOrWhiteSpace(newDevice.Tag) || newDevice.RoomId == 0)
            {
                Snackbar.Add(string.IsNullOrWhiteSpace(newDevice.Tag) ? "Device name is required." : "Please select a room.", Severity.Warning);
                return;
            }
            bool exists = devices.Any(d =>
             d.Tag.Equals(newDevice.Tag.Trim(), StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Snackbar.Add("Device name already exists.", Severity.Error);
                return;
            }

            if (await DeviceService.AddDeviceAsync(newDevice))
            {
                Snackbar.Add("Device added successfully!", Severity.Success);
                newDevice = new();
                await LoadDevices();
            }
            else
            {
                Snackbar.Add("Failed to add device.", Severity.Error);
            }
        }

        protected async Task UpdateDialog(DeviceDTO device)
        {
            var parameters = new DialogParameters
            {
                { "device", device },
                { "rooms", rooms },
                { "devices", devices }
            };

            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };

            var dialog = DialogService.Show<UpdateDeviceDialog>("Edit Device", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadDevices();
            }
        }

        protected async Task<bool> ConfirmDelete()
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to delete the selected device(s)?" },
                { "ButtonText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions
            {
                Position = DialogPosition.TopCenter,
                CloseOnEscapeKey = true,
                CloseButton = true
            };

            var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm Deletion", parameters, options);
            var result = await dialog.Result;

            return !result.Canceled;
        }

        protected async Task DeleteSelectedDevices()
        {
            if (!selectedDevices.Any())
            {
                Snackbar.Add("No devices selected.", Severity.Warning);
                return;
            }

            bool confirmed = await ConfirmDelete();
            if (!confirmed) return;

            foreach (var device in selectedDevices.ToList())
            {
                var success = await DeviceService.DeleteDeviceAsync(device.DeviceID);
                if (success)
                {
                    Snackbar.Add($"Deleted Device: {device.Tag}", Severity.Success);
                    devices.Remove(device);
                }
                else
                {
                    Snackbar.Add($"Failed to delete Device: {device.Tag}", Severity.Error);
                }
            }

            selectedDevices.Clear();
            StateHasChanged();
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

        protected void OnSelectionChanged(HashSet<DeviceDTO> selected)
        {
            selectedDevices = selected;
        }
    }
}