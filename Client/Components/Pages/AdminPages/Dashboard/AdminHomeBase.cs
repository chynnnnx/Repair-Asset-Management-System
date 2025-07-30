using Microsoft.AspNetCore.Components;
using Client.Services.Interfaces;
using Shared.DTOs;
using MudBlazor;
using Shared.Enums;

namespace Client.Components.Pages.AdminPages.Dashboard
{
    public class AdminHomeBase : ComponentBase
    {
        [Inject] protected IAuthService AuthService { get; set; } = default!;
        [Inject] protected IDeviceService DeviceService { get; set; } = default!;
        [Inject] protected IRoomService RoomService { get; set; } = default!;
        [Inject] protected IRepairRequestService RepairRequestService { get; set; } = default!;

        protected int totalUsers = 0;
        protected int adminUsers = 0;
        protected int totalRooms = 0;
        protected int totalDevices = 0;
        protected int totalRepairRequests = 0;
        protected bool isLoading = true;

        protected List<UserDTO> recentUsers = new();
        protected List<RepairRequestDTO> recentRepairRequests = new();

        protected int onlineDevices = 0;
        protected int offlineDevices = 0;
        protected int maintenanceDevices = 0;
        protected int notRespondingDevices = 0;

        protected int pendingRepairs = 0;
        protected int inProgressRepairs = 0;
        protected int completedRepairs = 0;

        protected override async Task OnInitializedAsync()
        {
            await LoadDashboardData();
        }

        protected async Task LoadDashboardData()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                
                var usersTask = AuthService.GetAllUsers();
                var roomsTask = RoomService.GetAllRoomsAsync();
                var devicesTask = DeviceService.GetAllDevicesAsync();
                var repairRequestsTask = RepairRequestService.GetAllRepairRequestsAsync();

                await Task.WhenAll(usersTask, roomsTask, devicesTask, repairRequestsTask);

    
                var users = await usersTask;
                var rooms = await roomsTask;
                var devices = await devicesTask;
                var repairRequests = await repairRequestsTask;

             
                totalUsers = users.Count;
                adminUsers = users.Count(u => u.Role == UserRole.Admin);
                totalRooms = rooms.Count;
                totalDevices = devices.Count;
                totalRepairRequests = repairRequests.Count;

                recentUsers = users
                    .OrderByDescending(u => u.UserID) 
                    .Take(5)
                    .ToList();

                recentRepairRequests = repairRequests
                    .OrderByDescending(r => r.RepairId) 
                    .Take(5)
                    .ToList();

                onlineDevices = devices.Count(d => d.Status == DeviceStatus.Online);
                offlineDevices = devices.Count(d => d.Status == DeviceStatus.Offline);
                maintenanceDevices = devices.Count(d => d.Status == DeviceStatus.Maintenance);
                notRespondingDevices = devices.Count(d => d.Status ==DeviceStatus.NotResponding);

                pendingRepairs = repairRequests.Count(r => r.Status == RepairStatus.Pending);
                inProgressRepairs = repairRequests.Count(r => r.Status ==RepairStatus.InProgress);
                completedRepairs = repairRequests.Count(r => r.Status == RepairStatus.Fixed || r.Status == RepairStatus.Replaced);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error loading dashboard data: {ex.Message}");

                totalUsers = 0;
                adminUsers = 0;
                totalRooms = 0;
                totalDevices = 0;
                totalRepairRequests = 0;
                recentUsers = new();
                recentRepairRequests = new();

              
                onlineDevices = 0;
                offlineDevices = 0;
                maintenanceDevices = 0;
                notRespondingDevices = 0;

                pendingRepairs = 0;
                inProgressRepairs = 0;
                completedRepairs = 0;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async Task RefreshData()
        {
            await LoadDashboardData();
        }

        protected Color GetStatusColor(string status)
        {
            return status?.ToLower() switch
            {
                "pending" => Color.Warning,
                "in progress" => Color.Info,
                "completed" => Color.Success,
                "cancelled" => Color.Error,
                _ => Color.Default
            };
        }
    }
}