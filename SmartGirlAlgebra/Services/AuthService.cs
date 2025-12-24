using System.Net.Http.Json;
using System.Net.Http.Headers;
using SmartGirlAlgebra.Models;
using Microsoft.JSInterop;

namespace SmartGirlAlgebra.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";
    private const string UserEmailKey = "userEmail";
    private const string UserDisplayNameKey = "userDisplayName";

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public event Action? OnAuthStateChanged;

    public async Task<bool> RegisterAsync(string email, string password, string displayName)
    {
        try
        {
            var request = new RegisterRequest
            {
                Email = email,
                Password = password,
                DisplayName = displayName
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null)
                {
                    await SaveAuthDataAsync(authResponse);
                    SetAuthorizationHeader(authResponse.Token);
                    OnAuthStateChanged?.Invoke();
                    return true;
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null)
                {
                    await SaveAuthDataAsync(authResponse);
                    SetAuthorizationHeader(authResponse.Token);
                    OnAuthStateChanged?.Invoke();
                    return true;
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserEmailKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserDisplayNameKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        OnAuthStateChanged?.Invoke();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
    }

    public async Task<string?> GetUserEmailAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", UserEmailKey);
    }

    public async Task<string?> GetUserDisplayNameAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", UserDisplayNameKey);
    }

    public async Task InitializeAsync()
    {
        var token = await GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            SetAuthorizationHeader(token);
        }
    }

    private async Task SaveAuthDataAsync(AuthResponse authResponse)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, authResponse.Token);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserEmailKey, authResponse.Email);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserDisplayNameKey, authResponse.DisplayName);
    }

    private void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}

