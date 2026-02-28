using System.Net.Http.Json;
using GarageVelo.Models;

namespace GarageVelo.Services.Api;

public class ApiAuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ISessionService _sessionService;
    private User? _currentUser;

    public ApiAuthService(HttpClient httpClient, ISessionService sessionService)
    {
        _httpClient = httpClient;
        _sessionService = sessionService;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { email, password });
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result == null)
            return null;

        await _sessionService.SaveTokenAsync(result.Token);
        _currentUser = new User
        {
            Id = result.User.Id,
            Email = result.User.Email,
            FirstName = result.User.FirstName,
            LastName = result.User.LastName
        };
        return _currentUser;
    }

    public async Task<User?> RegisterAsync(string email, string password, string firstName, string lastName)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register",
            new { email, password, firstName, lastName });
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result == null)
            return null;

        await _sessionService.SaveTokenAsync(result.Token);
        _currentUser = new User
        {
            Id = result.User.Id,
            Email = result.User.Email,
            FirstName = result.User.FirstName,
            LastName = result.User.LastName
        };
        return _currentUser;
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _httpClient.PostAsync("api/auth/logout", null);
        }
        catch { /* ignore network errors on logout */ }

        _currentUser = null;
        await _sessionService.ClearTokenAsync();
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
            return _currentUser;

        var isAuth = await _sessionService.IsAuthenticatedAsync();
        if (!isAuth)
            return null;

        try
        {
            var response = await _httpClient.GetAsync("api/auth/me");
            if (!response.IsSuccessStatusCode)
                return null;

            var userDto = await response.Content.ReadFromJsonAsync<UserResponse>();
            if (userDto == null)
                return null;

            _currentUser = new User
            {
                Id = userDto.Id,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };
            return _currentUser;
        }
        catch
        {
            return null;
        }
    }

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserResponse User { get; set; } = null!;
    }

    private class UserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
