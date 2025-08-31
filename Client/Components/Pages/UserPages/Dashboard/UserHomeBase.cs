using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Services.Interfaces;
using Shared.DTOs;
using MudBlazor;
using System.Security.Claims;
using Client.ViewModels;

namespace Client.Components.Pages.UserPages.Dashboard
{
    public class UserHomeBase : ComponentBase
    {
        [Inject] protected IDeviceService DeviceService { get; set; } = default!;
        [Inject] protected IRoomService RoomService { get; set; } = default!;
        [Inject] protected IRepairRequestService RepairRequestService { get; set; } = default!;
        [Inject] protected AuthenticationStateProvider AuthProvider { get; set; } = default!;

        protected string currentUserName = "User";
        protected int currentUserId = 0;
        protected int myRepairRequests = 0;
        protected int myPendingRequests = 0;
        protected int totalRooms = 0;
        protected int totalDevices = 0;
        protected bool isLoading = true;

        protected List<RepairRequestViewModel> userRepairRequests = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadCurrentUser();
            await LoadUserDashboardData();
        }

        protected async Task LoadCurrentUser()
        {
            try
            {
                var authState = await AuthProvider.GetAuthenticationStateAsync();
                if (authState.User.Identity?.IsAuthenticated == true)
                {
                    currentUserName = authState.User.FindFirst(ClaimTypes.Name)?.Value ?? "User";
                    var userIdClaim = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        currentUserId = userId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading current user: {ex.Message}");
            }
        }

        protected async Task LoadUserDashboardData()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                var devicesTask = DeviceService.GetAllDevicesAsync();
                var roomsTask = RoomService.GetAllRoomsAsync();
                var userRequestsTask = currentUserId > 0
                    ? RepairRequestService.GetRequestByUserIdAsync(currentUserId)
                    : Task.FromResult(new List<RepairRequestViewModel>());

                await Task.WhenAll(roomsTask, userRequestsTask, devicesTask);

                var rooms = await roomsTask;
                var devices = await devicesTask;

                var userRequests = (await userRequestsTask).ToList();

                totalRooms = rooms.Count;
                totalDevices = devices.Count;

                userRepairRequests = userRequests.OrderByDescending(r => r.RepairId).ToList();
                myRepairRequests = userRepairRequests.Count;
                myPendingRequests = userRepairRequests.Count(r =>
                    r.Status == Shared.Enums.RepairStatus.Pending ||
                    r.Status == Shared.Enums.RepairStatus.InProgress);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user dashboard data: {ex.Message}");

                totalRooms = 0;
                totalDevices = 0;

                myRepairRequests = 0;
                myPendingRequests = 0;
                userRepairRequests = new();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

      

    }
}
