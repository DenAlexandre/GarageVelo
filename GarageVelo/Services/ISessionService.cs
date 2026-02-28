namespace GarageVelo.Services;

public interface ISessionService
{
    Task SaveTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task ClearTokenAsync();
    Task<bool> IsAuthenticatedAsync();
}
