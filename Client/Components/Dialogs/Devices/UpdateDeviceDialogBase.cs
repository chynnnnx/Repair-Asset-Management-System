using Microsoft.AspNetCore.Components;
using MudBlazor;
using Client.Services.Interfaces;
using Shared.DTOs;
using Shared.Enums;
using Client.ViewModels;

namespace Client.Components.Dialogs.Devices
{
    public class UpdateDeviceDialogBase : ComponentBase
    {
        [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public DeviceViewModel device { get; set; } = new();
        [Parameter] public List<RoomViewModel> rooms { get; set; } = new();
        [Parameter] public List<DeviceViewModel> devices { get; set; } = new();
        [Inject] public IDeviceService DeviceService { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        protected MudTextField<string> deviceNameField = default!;
        protected bool isLoading = false;

        private string? originalTag;
        private int originalRoomId;
        private DeviceStatus originalStatus;

        protected override void OnInitialized()
        {
            originalTag = device.Tag?.Trim();
            originalRoomId = device.RoomId;
            originalStatus = device.Status;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && deviceNameField != null)
            {
                await Task.Delay(100);
                await deviceNameField.FocusAsync();
            }
        }

        protected bool HasChanges()
        {
            var currentName = device.Tag?.Trim();
            return !(currentName == originalTag &&
                     device.RoomId == originalRoomId &&
                     device.Status == originalStatus);
        }

        protected bool IsFormValid()
        {
            var currentName = device.Tag?.Trim();
            if (string.IsNullOrWhiteSpace(currentName)) return false;

            bool tagExists = devices.Any(d =>
                d.DeviceID != device.DeviceID &&
                d.Tag.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            return !tagExists && device.RoomId > 0;
        }

        protected Severity GetChangesSeverity()
        {
            if (!IsFormValid()) return Severity.Error;
            if (!HasChanges()) return Severity.Info;
            return Severity.Success;
        }

        protected string GetChangesIcon()
        {
            if (!IsFormValid()) return Icons.Material.Filled.Error;
            if (!HasChanges()) return Icons.Material.Filled.Info;
            return Icons.Material.Filled.CheckCircle;
        }

        protected string GetChangesTitle()
        {
            if (string.IsNullOrWhiteSpace(device.Tag?.Trim())) return "Device Name Required";

            bool tagExists = devices.Any(d =>
                d.DeviceID != device.DeviceID &&
                d.Tag.Equals(device.Tag?.Trim(), StringComparison.OrdinalIgnoreCase));

            if (tagExists) return "Duplicate Device Name";
            if (!HasChanges()) return "No Changes Detected";
            return "Ready to Update";
        }

        protected string GetChangesMessage()
        {
            if (string.IsNullOrWhiteSpace(device.Tag?.Trim()))
                return "Please enter a device name";

            bool tagExists = devices.Any(d =>
                d.DeviceID != device.DeviceID &&
                d.Tag.Equals(device.Tag?.Trim(), StringComparison.OrdinalIgnoreCase));

            if (tagExists)
                return "A device with this name already exists. Please choose a different name.";
            if (!HasChanges())
                return "No modifications have been made to the device details";

            var changes = new List<string>();
            if (device.Tag?.Trim() != originalTag) changes.Add("name");
            if (device.RoomId != originalRoomId) changes.Add("room");
            if (device.Status != originalStatus) changes.Add("status");

            return $"Changes detected in: {string.Join(", ", changes)}";
        }

        protected string GetStatusIcon(DeviceStatus status) => status switch
        {
            DeviceStatus.Online => Icons.Material.Filled.CheckCircle,
            DeviceStatus.Offline => Icons.Material.Filled.Cancel,
            DeviceStatus.Maintenance => Icons.Material.Filled.Build,
            DeviceStatus.NotResponding => Icons.Material.Filled.Archive,
            _ => Icons.Material.Filled.Help
        };

        protected Color GetStatusColor(DeviceStatus status) => status switch
        {
            DeviceStatus.Online => Color.Success,
            DeviceStatus.Offline => Color.Error,
            DeviceStatus.Maintenance => Color.Warning,
            DeviceStatus.NotResponding => Color.Default,
            _ => Color.Info
        };

        protected async Task UpdateDevice()
        {
            var currentName = device.Tag?.Trim();

            if (string.IsNullOrWhiteSpace(currentName))
            {
                Snackbar.Add("Device name is required.", Severity.Warning);
                await deviceNameField.FocusAsync();
                return;
            }

            bool tagExists = devices.Any(d =>
                d.DeviceID != device.DeviceID &&
                d.Tag.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            if (tagExists)
            {
                Snackbar.Add("Device name already exists. Please choose a different name.", Severity.Error);
                await deviceNameField.FocusAsync();
                return;
            }

            if (!HasChanges())
            {
                Snackbar.Add("No changes made.", Severity.Info);
                MudDialog.Cancel();
                return;
            }

            isLoading = true;
            StateHasChanged();

            try
            {
                device.Tag = currentName;
                var success = await DeviceService.UpdateDeviceAsync(device);

                if (success)
                {
                    Snackbar.Add($"Device '{device.Tag}' has been successfully updated!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    Snackbar.Add("Update failed. Please try again.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error updating device: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        protected void Cancel() => MudDialog.Cancel();
    }
}
