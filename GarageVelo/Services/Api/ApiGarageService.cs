using GarageVelo.Models;

namespace GarageVelo.Services.Api;

/// <summary>
/// HTTP-based garage service ready for real API integration.
/// </summary>
public class ApiGarageService : IGarageService
{
    private readonly HttpClient _httpClient;

    public ApiGarageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Garage?> GetByQrCodeAsync(string qrPayload)
        => throw new NotImplementedException("API not connected yet");

    public Task<List<Garage>> GetNearbyAsync(double latitude, double longitude)
        => throw new NotImplementedException("API not connected yet");

    public Task<Garage?> GetByIdAsync(string id)
        => throw new NotImplementedException("API not connected yet");
}
