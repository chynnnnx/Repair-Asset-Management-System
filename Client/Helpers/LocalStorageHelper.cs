using Blazored.LocalStorage;
using Shared.DTOs.Auth;

namespace Client.Helpers
{
    public static class LocalStorageHelper
    {
        public static async Task SavePCLoginSessionAsync(ILocalStorageService localStorage, string token, int userId, string tag, int roomId)
        {
            await localStorage.SetItemAsync(SessionKeys.AuthToken, token);
            await localStorage.SetItemAsync(SessionKeys.SessionUserId, userId);
            await localStorage.SetItemAsync(SessionKeys.Tag, tag);
            await localStorage.SetItemAsync(SessionKeys.RoomId, roomId);
            await localStorage.SetItemAsync(SessionKeys.IsPCLogin, true);
        }


        public static async Task ClearPCLoginSessionAsync(ILocalStorageService localStorage)
        {
            await localStorage.RemoveItemAsync(SessionKeys.AuthToken);
            await localStorage.RemoveItemAsync(SessionKeys.SessionUserId);
            await localStorage.RemoveItemAsync(SessionKeys.RoomId);
            await localStorage.RemoveItemAsync(SessionKeys.Tag);
            await localStorage.RemoveItemAsync(SessionKeys.IsPCLogin);
        }

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
