using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs;
using Client.Services.Interfaces;
using Client.Components.Dialogs.UserManagement;
using Client.Components.Pages.Common;
using Shared.Enums;
using System.Diagnostics;
using Client.ViewModels;

namespace Client.Components.Pages.AdminPages.UserManagement
{
    public class ManageUserBase : ComponentBase
    {
        [Inject] protected IAuthService userService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected List<UserViewModel> users = new();
        protected HashSet<UserViewModel> selectedUser= new();

        protected override async Task OnInitializedAsync()
        {
            await LoadUsers();
        }
        protected async Task LoadUsers()
        {
            users = await userService.GetAllUsers();
        }
        protected void OnSelectionChanged(HashSet<UserViewModel> selected)
        {
            selectedUser= selected;
        }

        protected string GetRoleLabel(UserRole role) => role switch
        {
            UserRole.Admin => "Admin",
            UserRole.User => "User",
          
            _ => "Unknown"
        };

        protected Color GetRoleColor(UserRole role) => role switch
        {
            UserRole.User => Color.Success,
            UserRole.Admin => Color.Info,
          
            _ => Color.Default
        };

        protected async Task DeleteSelectedUsers()
        {
            if (!selectedUser.Any())
            {
                Snackbar.Add("No user selected.", Severity.Warning);
                return;
            }

            bool confirmed = await ConfirmDelete();
            if (!confirmed) return;

            foreach (var user in selectedUser.ToList())
            {
                var success = await userService.DeleteUser(user.UserID);
                if (success)
                {
                    Snackbar.Add($"Deleted User: {user.FirstName}", Severity.Success);
                    users.Remove(user);
                }
                else
                {
                    Snackbar.Add($"Failed to delete User: {user.FirstName}", Severity.Error);
                }
            }

            selectedUser.Clear();
            StateHasChanged();
        }

        protected async Task<bool> ConfirmDelete()
        {
            var parameters = new DialogParameters
    {
        { "ContentText", "Are you sure you want to delete the selected user(s)?" },
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

        protected async Task Update(UserViewModel user)
        {
           
                var parameters = new DialogParameters
                {
                    ["user"] = user
                };

                var options = new DialogOptions { CloseOnEscapeKey = true };

                var dialogReference = await DialogService.ShowAsync<UpdateUserInfo>("Edit User", parameters, options);
             

                var result = await dialogReference.Result;

                if (!result.Canceled && result.Data is UserViewModel updatedUser)
                {
                    var success = await userService.UpdateUserInfo(updatedUser);
                    if (success)
                    {
                        Snackbar.Add("User updated successfully!", Severity.Success);
                        await LoadUsers();
                    }
                    else
                    {
                        Snackbar.Add("Failed to update user.", Severity.Error);
                    }
                }
            StateHasChanged();
        }
          

        protected async Task OpenAddUserDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };
            var dialog = await DialogService.ShowAsync<AddUserOrAdminDialog>("AddUserOrAdminDialog", options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {

                await LoadUsers();
            }
        }


    }
}

