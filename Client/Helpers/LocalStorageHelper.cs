using Blazored.LocalStorage;
using Shared.DTOs.Auth;

namespace Client.Helpers
{
    public static class LocalStorageHelper
    {
      
        public static async Task SaveWebLoginSessionAsync(ILocalStorageService localStorage, LoginResultDTO login)
        {
            await localStorage.SetItemAsync(SessionKeys.AuthToken, login.Token);
            await localStorage.SetItemAsync(SessionKeys.SessionUserId, login.UserId);
            await localStorage.SetItemAsync(SessionKeys.SessionEmail, login.Email);
            await localStorage.SetItemAsync(SessionKeys.SessionRole, login.Role);
        }

        public static async Task ClearWebLoginSessionAsync(ILocalStorageService localStorage)
        {
            await localStorage.RemoveItemAsync(SessionKeys.AuthToken);
            await localStorage.RemoveItemAsync(SessionKeys.SessionUserId);
            await localStorage.RemoveItemAsync(SessionKeys.SessionEmail);
            await localStorage.RemoveItemAsync(SessionKeys.SessionRole);
        }

        public static async Task ClearAllSessionsAsync(ILocalStorageService localStorage)
        {
            await ClearWebLoginSessionAsync(localStorage);
            await ClearPCLoginSessionAsync(localStorage);
        }



    }
}
