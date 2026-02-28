using GarageVelo.Models;

namespace GarageVelo.Services;

public interface IAuthService
{
    Task<User?> LoginAsync(string email, string password);
    Task<User?> RegisterAsync(string email, string password, string firstName, string lastName);
    Task LogoutAsync();
    Task<User?> GetCurrentUserAsync();
}
