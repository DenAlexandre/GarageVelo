using System.Net.Http.Json;
using GarageVelo.Models;

namespace GarageVelo.Services.Api;

public class ApiGarageService : IGarageService
{
    private readonly HttpClient _httpClient;

    public ApiGarageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Garage?> GetByQrCodeAsync(string qrPayload)
    {
        var encoded = Uri.EscapeDataString(qrPayload);
        var response = await _httpClient.GetAsync($"api/garages/qr/{encoded}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<Garage>();
    }

    public async Task<List<Garage>> GetNearbyAsync(double latitude, double longitude)
    {
        var garages = await _httpClient.GetFromJsonAsync<List<Garage>>("api/garages");
        return garages ?? [];
    }

    public async Task<Garage?> GetByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"api/garages/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<Garage>();
    }
}
