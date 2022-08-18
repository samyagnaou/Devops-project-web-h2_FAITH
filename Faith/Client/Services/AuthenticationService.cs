using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Faith.Client.AuthProviders;
using Faith.Client.Interfaces;
using Faith.Shared.Models.Requests;
using Faith.Shared.Models.Responses;
using Intersoft.Crosslight.Mobile;
using Microsoft.AspNetCore.Components.Authorization;

namespace Faith.Client.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(
        HttpClient client,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {
        _client = client;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest request)
    {
        var content = JsonSerializer.Serialize(request);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

        var authResult = await _client.PostAsync("accounts/login", bodyContent);
        var authContent = await authResult.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<UserLoginResponse>(authContent, _options);

        if (!authResult.IsSuccessStatusCode)
            return result;

        await _localStorage.SetItemAsync("authToken", result.Token);
        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
        return new UserLoginResponse { IsAuthSuccessful = true };
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _client.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
    {
        var content = JsonSerializer.Serialize(request);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");

        var registrationResult = await _client.PostAsync("/accounts/register", bodyContent);
        var registrationContent = await registrationResult.Content.ReadAsStringAsync();

        if (!registrationResult.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<RegisterUserResponse>(registrationContent, _options);
            return result;
        }

        return new RegisterUserResponse { IsSuccessfulRegistration = true };
    }
}