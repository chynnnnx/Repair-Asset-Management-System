using Microsoft.AspNetCore.Components;
using MudBlazor;
using Client.ViewModels;
using Microsoft.AspNetCore.Components.Web;

namespace Client.Components.Dialogs.RoomOffices
{
    public class UpdateRoomDialogBase : ComponentBase
    {
        [Parameter] public RoomViewModel Room { get; set; } = new();
        [Parameter] public List<RoomViewModel> Rooms { get; set; } = new();
        [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected string? originalName;
        protected bool isLoading = false;
        protected MudTextField<string> roomNameField = default!;

        protected override void OnInitialized()
        {
            originalName = Room.RoomName?.Trim();
        }

        protected bool HasChanges()
        {
            var currentName = Room.RoomName?.Trim();
            return !string.Equals(currentName, originalName, StringComparison.OrdinalIgnoreCase);
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
            var newName = Room.RoomName?.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                Snackbar.Add("Room name is required.", Severity.Warning);
                await roomNameField.FocusAsync();
                return;
            }

            if (newName == originalName)
            {
                Snackbar.Add("No changes made.", Severity.Info);
                MudDialog.Cancel();
                return;
            }

            bool nameExists = Rooms.Any(r =>
                r.RoomId != Room.RoomId &&
                r.RoomName.Equals(newName, StringComparison.OrdinalIgnoreCase));

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
                Room.RoomName = newName;
                Snackbar.Add($"Room '{newName}' has been successfully updated!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(Room));
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
