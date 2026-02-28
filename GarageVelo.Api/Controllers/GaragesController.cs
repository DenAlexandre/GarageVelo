using System.Text;
using System.Text.Json;
using GarageVelo.Api.Data;
using GarageVelo.Api.DTOs;
using GarageVelo.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageVelo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GaragesController : ControllerBase
{
    private readonly AppDbContext _db;

    public GaragesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var garages = await _db.Garages.Include(g => g.Site).ToListAsync();
        return Ok(garages.Select(MapDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var garage = await _db.Garages.Include(g => g.Site).FirstOrDefaultAsync(g => g.Id == id);
        if (garage == null)
            return NotFound(new { message = "Garage non trouvé." });

        return Ok(MapDto(garage));
    }

    [HttpGet("qr/{qrData}")]
    public async Task<IActionResult> GetByQrCode(string qrData)
    {
        string garageId;
        try
        {
            // Try base64 decode first
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(qrData));
            var payload = JsonSerializer.Deserialize<QrPayload>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            garageId = payload?.Id ?? qrData;
        }
        catch
        {
            // Try parsing as raw JSON
            try
            {
                var payload = JsonSerializer.Deserialize<QrPayload>(qrData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                garageId = payload?.Id ?? qrData;
            }
            catch
            {
                // Fallback: treat as direct garage ID
                garageId = qrData;
            }
        }

        var garage = await _db.Garages.Include(g => g.Site).FirstOrDefaultAsync(g => g.Id == garageId);
        if (garage == null)
            return NotFound(new { message = "Garage non trouvé pour ce QR code." });

        return Ok(MapDto(garage));
    }

    private static GarageDto MapDto(GarageEntity g) => new()
    {
        Id = g.Id,
        Name = g.Name,
        SiteId = g.SiteId,
        SiteName = g.Site.Name,
        Address = g.Site.Address,
        Latitude = g.Site.Latitude,
        Longitude = g.Site.Longitude,
        Size = g.Size,
        TotalSlots = g.TotalSlots,
        AvailableSlots = g.AvailableSlots,
        Position = g.Position,
        LockCode = g.LockCode
    };

    private class QrPayload
    {
        public string Id { get; set; } = string.Empty;
        public int Pos { get; set; }
        public string Lock { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
    }
}
