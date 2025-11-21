using Blazored.LocalStorage;
using Client.Helpers;
using Client.Security;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Shared.DTOs.Auth; 
using System.Threading.Tasks;

namespace Client.Components.Pages.Authentication
{
    public partial class SharedAuthenticationBase : ComponentBase
    {
        [Inject] protected IAuthService AuthService { get; set; } = null!;
        [Inject] protected NavigationManager Navigation { get; set; } = null!;
        [Inject] protected ILocalStorageService LocalStorage { get; set; } = null!;
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject] protected ISnackbar Snackbar { get; set; } = null!;

        protected int activeStep = 0;
        protected RegisterUserDTO registerUser = new();
        protected bool isTermsAccepted = false;
        protected bool isLoading = false;
        protected bool _showPassword = false;

        protected string _email;
        protected string _password;
        protected string _error;
      
        protected void TogglePasswordVisibility()
        {
            _showPassword = !_showPassword;
        }
        protected bool Step1Completed =>
            !string.IsNullOrWhiteSpace(registerUser.FirstName) &&
            !string.IsNullOrWhiteSpace(registerUser.LastName) &&
            !string.IsNullOrWhiteSpace(registerUser.Gender) &&
            new[] { "Male", "Female", "Other" }.Contains(registerUser.Gender);

        protected bool Step2Completed =>
            !string.IsNullOrWhiteSpace(registerUser.Email) &&
            !string.IsNullOrWhiteSpace(registerUser.PasswordHash);

        protected async Task SubmitRegistration()
        {
            if (!Step1Completed || !Step2Completed)
            {
                Snackbar.Add("Please fill in all required fields before proceeding.", Severity.Warning);
                return;
            }

            if (!isTermsAccepted)
            {
                Snackbar.Add("Please agree to the Terms and Conditions before proceeding.", Severity.Warning);
                return;
            }

            isLoading = true;

            try
            {
                await Task.Delay(2000);
                var error = await AuthService.RegisterAccount(registerUser);
                if (error == null)
                {
                    Snackbar.Add("Account created! Please login.", Severity.Success);
                    Navigation.NavigateTo("/");
                }
                else
                {
                    Snackbar.Add(error, Severity.Error);
                }
            }
            catch (Exception)
            {
                Snackbar.Add("Something went wrong during registration.", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }
        protected async Task LoginAccount()
        {
            _error = "";
            isLoading = true;
            try
            {
                var result = await AuthService.LoginAsync(new LoginDTO
                {
                    Email = _email,
                    PasswordHash = _password
                });
                if (string.IsNullOrEmpty(result.Token))
                {
                    _error = "Invalid credentials.";
                    return;
                }
                await LocalStorageHelper.SaveWebLoginSessionAsync(LocalStorage, result);
                if (AuthStateProvider is JwtAuthenticationStateProvider jwtProvider)
                {
                    jwtProvider.NotifyUserAuthentication(result.Token);
                }
                Navigation.NavigateTo(result.Role == "Admin" ? "/adminhome" : "/userhome");
            }
            catch
            {
                _error = "Login failed. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}

