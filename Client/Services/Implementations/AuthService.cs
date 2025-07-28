using Blazored.LocalStorage;
using Shared.DTOs.Auth;
using Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using Client.Security;
using Client.Helpers;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Client.Components.Dialogs;
using MudBlazor;
namespace Client.Services.Implementations
{
    public class AuthService : BaseHttpService, IAuthService
    {
        private readonly JwtAuthenticationStateProvider _authProvider;

        public AuthService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authProvider,
            ILogger<AuthService> logger)
            : base(httpClient, localStorage, logger)
        {
            _authProvider = (JwtAuthenticationStateProvider)authProvider;
        }

        public async Task<LoginResultDTO?> LoginAsync(LoginDTO loginDto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", loginDto);
            var result = await response.Content.ReadFromJsonAsync<LoginResultDTO>();

            if (result?.Token != null)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
                _authProvider.NotifyUserAuthentication(result.Token);
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
            }

            return result;
        }

        public async Task<string?> RegisterAccount(RegisterUserDTO registerDto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", registerDto);

            if (response.IsSuccessStatusCode)
                return null;

            var error = await response.Content.ReadAsStringAsync();
            return error;
        }

        public async Task LogoutAsync()
        {
            await LocalStorageHelper.ClearWebLoginSessionAsync(_localStorage);
            _authProvider.NotifyUserLogout();
        }


        public async Task<bool> AddUserAsync(RegisterUserDTO userDto)
        {
            var response = await _http.PostAsJsonAsync("api/auth", userDto);

            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error); // throw actual server error
        }
      

        public async Task <bool> UpdateUserInfo(UserDTO userDto)
        {
            return await PutAsync("api/auth", userDto);
          
        }
        
        public async Task <bool> DeleteUser(int userId)
        {
            return await DeleteAsync($"api/auth/{userId}");
        }
     public async Task<List<UserDTO>> GetAllUsers()
        {
            return await GetAsync<List<UserDTO>>("api/auth/users") ?? new();
        }

    }
}
