using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs;
using Client.Services.Interfaces;
using Client.Helpers;
using Blazored.LocalStorage;
using Shared.Enums;

namespace Client.Components.Pages.UserPages.Reports
{
    public class ReportRepairRequestBase : ComponentBase
    {

        [Inject] protected ILocalStorageService LocalStorage { get; set; }
        [Inject] protected IRepairRequestService RepairRequestService { get; set; }
        [Inject] protected IDeviceService DeviceService { get; set; }
        [Inject] protected ISnackbar Snackbar { get; set; }


        protected List<RepairRequestDTO> request = new();


        protected MudForm _form;
        protected RepairRequestDTO _repairRequest = new();
        protected List<DeviceDTO> _devices = new();
        protected string _selectedRoom = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _devices = await DeviceService.GetAllDevicesAsync();
            var userId = await LocalStorage.GetItemAsync<int>(SessionKeys.SessionUserId);
            request = (await RepairRequestService.GetRequestByUserIdAsync(userId)).ToList();
        }

        protected async Task OnDeviceChanged(int selectedDeviceId)
        {
            _repairRequest.DeviceId = selectedDeviceId;
            var selected = _devices.FirstOrDefault(d => d.DeviceID == selectedDeviceId);
            _selectedRoom = selected?.RoomName ?? "";
            await InvokeAsync(StateHasChanged);
        }

        protected async Task SubmitRequest()
        {
            var userId = await LocalStorage.GetItemAsync<int>(SessionKeys.SessionUserId);
            _repairRequest.ReportedByUserId = userId;

        

            await _form.Validate();
            if (_form.IsValid)
            {
                bool success = await RepairRequestService.AddRepairRequestAsync(_repairRequest);
                if (success)
                {
                    Snackbar.Add("Repair request submitted successfully!", Severity.Success);
                    _repairRequest = new();
                    _selectedRoom = "";
                }
                else
                {
                    Snackbar.Add("Failed to submit request. Try again.", Severity.Error);
                }
            }
        }

        protected string GetStatusLabel(RepairStatus status) => status switch
        {
            RepairStatus.Fixed => "Fixed",
            RepairStatus.Replaced => "Replaced",
            RepairStatus.Pending => "Pending",
            RepairStatus.InProgress => "In Progress",
            _ => "Unknown"
        };

        protected Color GetStatusColor(RepairStatus status) => status switch
        {
            RepairStatus.Fixed => Color.Success,
            RepairStatus.Replaced => Color.Error,
            RepairStatus.InProgress => Color.Info,
            RepairStatus.Pending => Color.Warning,
            _ => Color.Default
        };

        protected string GetStatusIcon(RepairStatus status) => status switch
        {
            RepairStatus.Fixed => Icons.Material.Filled.CheckCircle,
            RepairStatus.Replaced => Icons.Material.Filled.SwapHoriz,
            RepairStatus.InProgress => Icons.Material.Filled.HourglassTop,
            RepairStatus.Pending => Icons.Material.Filled.Schedule,
            _ => Icons.Material.Filled.Help
        };
    }
}
