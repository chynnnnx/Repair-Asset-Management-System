using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs;
using Shared.DTOs.Auth;
using System;
using System.Threading.Tasks;

namespace Client.Components.Dialogs.UserManagement
{
    public class AddEditUserDialogBase : ComponentBase
    {
        protected MudForm _form = default!;
        protected RegisterUserDTO newUser = new();
        protected bool isLoading = false;
        protected bool showPassword = false;
        protected InputType passwordInputType = InputType.Password;
        protected string passwordIcon = Icons.Material.Filled.VisibilityOff;
        protected MudForm form = default!;
        protected bool loading = false;
        [Parameter] public UserDTO user { get; set; } = new();
        [Parameter] public bool IsEditMode { get; set; } = false;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;

        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected IAuthService userService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;

        protected void TogglePasswordVisibility()
        {
            showPassword = !showPassword;
            passwordInputType = showPassword ? InputType.Text : InputType.Password;
            passwordIcon = showPassword ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;
        }

        protected async Task AddUser()
        {
            await _form.Validate();
            if (!_form.IsValid)
            {
                Snackbar.Add("Please fill in all required fields correctly.", Severity.Warning);
                return;
            }

            isLoading = true;
            StateHasChanged();

            try
            {
                var success = await userService.AddUserAsync(newUser);
                if (success)
                {
                    Snackbar.Add($"User {newUser.FirstName} {newUser.LastName} has been successfully added to RAMS!", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(newUser));
                }
                else
                {
                    Snackbar.Add("Failed to create user. Please check if the email is already registered.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error creating user: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        protected void Cancel() => MudDialog.Cancel();
        protected async Task Submit()
        {
            await form.Validate();

            if (form.IsValid)
            {
                loading = true;
                StateHasChanged();
                MudDialog.Close(DialogResult.Ok(user));
            }
        }

    }
}
