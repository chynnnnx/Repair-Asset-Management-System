using Microsoft.AspNetCore.Components;
using MudBlazor;
using Client.Services.Interfaces;
using Client.Components.Dialogs.RoomOffices;
using Client.Components.Pages.Common;
using Client.ViewModels;

namespace Client.Components.Pages.AdminPages.RoomsOrOffices
{
    public class RoomManagementBase : ComponentBase
    {
        [Inject] protected IRoomService RoomService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected string roomName = string.Empty;

        protected List<RoomViewModel> rooms = new();
        protected HashSet<RoomViewModel> selectedRooms = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadRooms();
        }

        protected async Task LoadRooms()
        {
            rooms = await RoomService.GetAllRoomsAsync();
        }

        protected async Task AddRoom()
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                Snackbar.Add("Room name is required.", Severity.Warning);
                return;
            }

            var trimmedName = roomName.Trim();

            bool exists = rooms.Any(r => r.RoomName.Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                Snackbar.Add("Room name already exists.", Severity.Error);
                return;
            }

            var newRoom = new RoomViewModel { RoomName = trimmedName };
            var success = await RoomService.AddRoomAsync(newRoom);

            if (success)
            {
                Snackbar.Add("Room added successfully!", Severity.Success);
                roomName = string.Empty;
                await LoadRooms();
            }
            else
            {
                Snackbar.Add("Failed to add room.", Severity.Error);
            }
        }

        protected void OnSelectionChanged(HashSet<RoomViewModel> selected)
        {
            selectedRooms = selected;
        }

        protected async Task Update(RoomViewModel room)
        {
            var parameters = new DialogParameters
            {
                ["room"] = new RoomViewModel
                {
                    RoomId = room.RoomId,
                    RoomName = room.RoomName
                },
                ["rooms"] = rooms
            };

            var options = new DialogOptions { CloseOnEscapeKey = true };
            var dialogReference = await DialogService.ShowAsync<UpdateRoomDialog>("Edit Room", parameters, options);
            var result = await dialogReference.Result;
            if (!result.Canceled && result.Data is RoomViewModel updatedRoom)
            {
                var success = await RoomService.UpdateRoomAsync(updatedRoom);

                if (success)
                {
                    await LoadRooms();
                }
                else
                {
                    Snackbar.Add("Failed to update room.", Severity.Error);
                }
            }

            StateHasChanged();
        }

        protected async Task<bool> ConfirmDelete()
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to delete the selected room(s)?" },
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

        protected async Task DeleteSelectedRooms()
        {
            if (!selectedRooms.Any())
            {
                Snackbar.Add("No rooms selected.", Severity.Warning);
                return;
            }

            bool confirmed = await ConfirmDelete();
            if (!confirmed) return;

            foreach (var room in selectedRooms.ToList())
            {
                var success = await RoomService.DeleteRoomAsync(room.RoomId);
                if (success)
                {
                    Snackbar.Add($"Deleted Room: {room.RoomName}", Severity.Success);
                    rooms.Remove(room);
                }
                else
                {
                    Snackbar.Add($"Failed to delete Room: {room.RoomName}", Severity.Error);
                }
            }

            selectedRooms.Clear();
            StateHasChanged();
        }
    }
}
