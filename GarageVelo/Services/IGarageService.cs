using GarageVelo.Models;

namespace GarageVelo.Services;

public interface IGarageService
{
    Task<Garage?> GetByQrCodeAsync(string qrPayload);
    Task<List<Garage>> GetNearbyAsync(double latitude, double longitude);
    Task<Garage?> GetByIdAsync(string id);
}
