using System.Text.Json;
using GarageVelo.Models;

namespace GarageVelo.Services.Mock;

public class MockGarageService : IGarageService
{
    private readonly List<Garage> _garages =
    [
        new()
        {
            Id = "GV-0001", Name = "Garage Bellecour",
            SiteId = "SITE-001", SiteName = "Bellecour",
            Address = "Place Bellecour, 69002 Lyon",
            Latitude = 45.7578, Longitude = 4.8320, Size = "Large",
            TotalSlots = 20, AvailableSlots = 8, LockCode = "814623"
        },
        new()
        {
            Id = "GV-0002", Name = "Garage Part-Dieu",
            SiteId = "SITE-002", SiteName = "Part-Dieu",
            Address = "Gare Part-Dieu, 69003 Lyon",
            Latitude = 45.7606, Longitude = 4.8602, Size = "Large",
            TotalSlots = 30, AvailableSlots = 15, LockCode = "927384"
        },
        new()
        {
            Id = "GV-0003", Name = "Garage Confluence",
            SiteId = "SITE-003", SiteName = "Confluence",
            Address = "Centre Confluence, 69002 Lyon",
            Latitude = 45.7432, Longitude = 4.8183, Size = "Medium",
            TotalSlots = 12, AvailableSlots = 5, LockCode = "536271"
        },
        new()
        {
            Id = "GV-0004", Name = "Garage Vieux Lyon",
            SiteId = "SITE-001", SiteName = "Bellecour",
            Address = "Place Bellecour, 69002 Lyon",
            Latitude = 45.7578, Longitude = 4.8320, Size = "Small",
            TotalSlots = 6, AvailableSlots = 2, LockCode = "148592"
        },
        new()
        {
            Id = "GV-0005", Name = "Garage Tête d'Or",
            SiteId = "SITE-002", SiteName = "Part-Dieu",
            Address = "Gare Part-Dieu, 69003 Lyon",
            Latitude = 45.7606, Longitude = 4.8602, Size = "Medium",
            TotalSlots = 10, AvailableSlots = 7, LockCode = "673810"
        }
    ];

    public async Task<Garage?> GetByQrCodeAsync(string qrPayload)
    {
        await Task.Delay(500);
        try
        {
            var data = JsonSerializer.Deserialize<QrPayload>(qrPayload, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data is null) return null;

            var garage = _garages.FirstOrDefault(g => g.Id == data.Id);
            if (garage is not null && data.Pos > 0)
                garage.Position = data.Pos;
            return garage;
        }
        catch
        {
            return _garages.FirstOrDefault(g => g.Id == qrPayload);
        }
    }

    public async Task<List<Garage>> GetNearbyAsync(double latitude, double longitude)
    {
        await Task.Delay(300);
        return _garages;
    }

    public async Task<Garage?> GetByIdAsync(string id)
    {
        await Task.Delay(200);
        return _garages.FirstOrDefault(g => g.Id == id);
    }

    private class QrPayload
    {
        public string Id { get; set; } = string.Empty;
        public int Pos { get; set; }
        public string Lock { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
    }
}
