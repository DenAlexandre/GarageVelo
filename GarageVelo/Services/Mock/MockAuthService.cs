using GarageVelo.Models;

namespace GarageVelo.Services.Mock;

public class MockAuthService : IAuthService
{
    private readonly ISessionService _sessionService;
    private User? _currentUser;

    private readonly List<(string Email, string Password, User User)> _users =
    [
        ("demo@garagevelo.fr", "password123", new User
        {
            Id = "USR-0001",
            Email = "demo@garagevelo.fr",
            FirstName = "Jean",
            LastName = "Dupont"
        })
    ];

    public MockAuthService(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        await Task.Delay(800); // Simulate network
        var match = _users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);

        if (match.User is null)
            return null;

        _currentUser = match.User;
        await _sessionService.SaveTokenAsync($"mock-token-{_currentUser.Id}");
        return _currentUser;
    }

    public async Task<User?> RegisterAsync(string email, string password, string firstName, string lastName)
    {
        await Task.Delay(800);

        if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return null;

        var user = new User
        {
            Id = $"USR-{_users.Count + 1:D4}",
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        _users.Add((email, password, user));
        _currentUser = user;
        await _sessionService.SaveTokenAsync($"mock-token-{user.Id}");
        return user;
    }

    public async Task LogoutAsync()
    {
        _currentUser = null;
        await _sessionService.ClearTokenAsync();
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var token = await _sessionService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return null;

        if (_currentUser is not null)
            return _currentUser;

        // Simulate restoring session from token
        var userId = token.Replace("mock-token-", "");
        _currentUser = _users.FirstOrDefault(u => u.User.Id == userId).User;
        return _currentUser;
    }
}
