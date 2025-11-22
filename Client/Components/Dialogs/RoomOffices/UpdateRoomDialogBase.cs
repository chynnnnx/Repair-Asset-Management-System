using Microsoft.AspNetCore.Components;
using MudBlazor;
using Client.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Client.Services.Interfaces;
namespace Client.Components.Dialogs.RoomOffices
{
    public class UpdateRoomDialogBase : ComponentBase
    {
        [Inject] protected IRoomService RoomService { get; set; } = default!;

        [Parameter] public RoomViewModel Room { get; set; } = new();
        [Parameter] public List<RoomViewModel> Rooms { get; set; } = new();
        [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected string? originalName;
        protected bool isLoading = false;
        protected MudTextField<string> roomNameField = default!;
        protected string? originalLocation;
        protected int originalCapacity;

        protected override void OnInitialized()
        {
            originalName = Room.RoomName?.Trim();
            originalLocation = Room.Location?.Trim();
            originalCapacity = Room.Capacity;
        }

     

        protected bool HasChanges()
        {

            return Room.RoomName?.Trim() != originalName
                   || Room.Location?.Trim() != originalLocation
                   || Room.Capacity != originalCapacity;
        }

        protected Severity GetAlertSeverity()
        {
            var currentName = Room.RoomName?.Trim();

            if (string.IsNullOrWhiteSpace(currentName))
                return Severity.Warning;

            if (!HasChanges())
                return Severity.Info;

            bool nameExists = Rooms.Any(r =>
                r.RoomId != Room.RoomId &&
                r.RoomName.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            return nameExists ? Severity.Error : Severity.Success;
        }

        protected string GetStatusIcon()
        {
            var currentName = Room.RoomName?.Trim();

            if (string.IsNullOrWhiteSpace(currentName))
                return Icons.Material.Filled.Warning;

            if (!HasChanges())
                return Icons.Material.Filled.Info;

            bool nameExists = Rooms.Any(r =>
                r.RoomId != Room.RoomId &&
                r.RoomName.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            return nameExists ? Icons.Material.Filled.Error : Icons.Material.Filled.CheckCircle;
        }

        protected string GetStatusMessage()
        {
            var currentName = Room.RoomName?.Trim();

            if (string.IsNullOrWhiteSpace(currentName))
                return "Room name is required";

            if (!HasChanges())
                return "No changes detected";

            bool nameExists = Rooms.Any(r =>
                r.RoomId != Room.RoomId &&
                r.RoomName.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            return nameExists ? "Room name already exists" : "Ready to save changes";
        }

        public async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Room.RoomName))
            {
                Snackbar.Add("Room name is required.", Severity.Warning);
                await roomNameField.FocusAsync();
                return;
            }

            if (!HasChanges())
            {
                Snackbar.Add("No changes made.", Severity.Info);
                MudDialog.Cancel();
                return;
            }

            bool nameExists = Rooms.Any(r =>
                r.RoomId != Room.RoomId &&
                r.RoomName.Equals(Room.RoomName?.Trim(), StringComparison.OrdinalIgnoreCase));

            if (nameExists)
            {
                Snackbar.Add("Room name already exists. Please choose a different name.", Severity.Error);
                await roomNameField.FocusAsync();
                await roomNameField.SelectAsync();
                return;
            }

            isLoading = true;
            StateHasChanged();

            try
            {
                var success = await RoomService.UpdateRoomAsync(Room); 
                if (success)
                {
                    Snackbar.Add($"Room '{Room.RoomName}' has been successfully updated!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(Room));
                }
                else
                {
                    Snackbar.Add("Failed to update room.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error updating room: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

    }
}
