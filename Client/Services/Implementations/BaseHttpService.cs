using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;

namespace Client.Services.Implementations
{
    public abstract class BaseHttpService
    {
        protected readonly HttpClient _http;
        protected readonly ILocalStorageService _localStorage;
        protected readonly ILogger<BaseHttpService> _logger;

        protected BaseHttpService(HttpClient http, ILocalStorageService localStorage, ILogger<BaseHttpService> logger)
        {
            _http = http;
            _localStorage = localStorage;
            _logger = logger;
        }

        protected async Task SetAuthHeaderAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                return await _http.GetFromJsonAsync<T>(url, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GET {url} failed");
                return default;
            }
        }

        protected async Task<bool> PostAsync<T>(string url, T data)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _http.PostAsJsonAsync(url, data);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"POST {url} failed");
                return false;
            }
        }

        protected async Task<bool> PutAsync<T>(string url, T data)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _http.PutAsJsonAsync(url, data);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"PUT {url} failed");
                return false;
            }
        }

        protected async Task<bool> DeleteAsync(string url)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _http.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DELETE {url} failed");
                return false;
            }
        }
        protected async Task<byte[]> GetFileAsync(string url)
        {
            await SetAuthHeaderAsync(); 
            var response = await _http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return Array.Empty<byte>();
        }

    }
}
