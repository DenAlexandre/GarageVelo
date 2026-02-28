using GarageVelo.Models;

namespace GarageVelo.Services.Api;

/// <summary>
/// HTTP-based auth service ready for real API integration.
/// Currently throws NotImplementedException — swap with mock in DI for development.
/// </summary>
public class ApiAuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ISessionService _sessionService;

    public ApiAuthService(HttpClient httpClient, ISessionService sessionService)
    {
        _httpClient = httpClient;
        _sessionService = sessionService;
    }

    public Task<User?> LoginAsync(string email, string password)
        => throw new NotImplementedException("API not connected yet");

    public Task<User?> RegisterAsync(string email, string password, string firstName, string lastName)
        => throw new NotImplementedException("API not connected yet");

    public Task LogoutAsync()
        => throw new NotImplementedException("API not connected yet");

    public Task<User?> GetCurrentUserAsync()
        => throw new NotImplementedException("API not connected yet");
}
