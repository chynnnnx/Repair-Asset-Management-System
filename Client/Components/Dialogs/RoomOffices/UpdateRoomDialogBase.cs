using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs;
using Microsoft.AspNetCore.Components.Web;

namespace Client.Components.Dialogs.RoomOffices
{
    public class UpdateRoomDialogBase : ComponentBase
    {
        [Parameter] public RoomDTO room { get; set; } = new();
        [Parameter] public List<RoomDTO> rooms { get; set; } = new();
        [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected string? originalName;
        protected bool isLoading = false;
        protected MudTextField<string> roomNameField = default!;

        protected override void OnInitialized()
        {
            originalName = room.RoomName?.Trim();
        }

        protected bool HasChanges()
        {
            var currentName = room.RoomName?.Trim();
            return !string.Equals(currentName, originalName, StringComparison.OrdinalIgnoreCase);
        }

        protected Severity GetAlertSeverity()
        {
            var currentName = room.RoomName?.Trim();

            if (string.IsNullOrWhiteSpace(currentName))
                return Severity.Warning;

            if (!HasChanges())
                return Severity.Info;

            bool nameExists = rooms.Any(r =>
                r.RoomId != room.RoomId &&
                r.RoomName.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            return nameExists ? Severity.Error : Severity.Success;
        }

        protected string GetStatusIcon()
        {
            var currentName = room.RoomName?.Trim();

            if (string.IsNullOrWhiteSpace(currentName))
                return Icons.Material.Filled.Warning;

            if (!HasChanges())
                return Icons.Material.Filled.Info;

            bool nameExists = rooms.Any(r =>
                r.RoomId != room.RoomId &&
                r.RoomName.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            return nameExists ? Icons.Material.Filled.Error : Icons.Material.Filled.CheckCircle;
        }

        protected string GetStatusMessage()
        {
            var currentName = room.RoomName?.Trim();

            if (string.IsNullOrWhiteSpace(currentName))
                return "Room name is required";

            if (!HasChanges())
                return "No changes detected";

            bool nameExists = rooms.Any(r =>
                r.RoomId != room.RoomId &&
                r.RoomName.Equals(currentName, StringComparison.OrdinalIgnoreCase));

            return nameExists ? "Room name already exists" : "Ready to save changes";
        }

        public async Task Save()
        {
            var newName = room.RoomName?.Trim();

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

            bool nameExists = rooms.Any(r =>
                r.RoomId != room.RoomId &&
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
                room.RoomName = newName;
                Snackbar.Add($"Room '{newName}' has been successfully updated!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(room));
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
