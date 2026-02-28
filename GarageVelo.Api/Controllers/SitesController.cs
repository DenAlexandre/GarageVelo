using GarageVelo.Api.Data;
using GarageVelo.Api.DTOs;
using GarageVelo.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageVelo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SitesController : ControllerBase
{
    private readonly AppDbContext _db;

    public SitesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sites = await _db.Sites.Include(s => s.Garages).ToListAsync();
        return Ok(sites.Select(MapDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var site = await _db.Sites.Include(s => s.Garages).FirstOrDefaultAsync(s => s.Id == id);
        if (site == null)
            return NotFound(new { message = "Site non trouvé." });

        return Ok(MapDto(site));
    }

    private static SiteDto MapDto(SiteEntity s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Address = s.Address,
        Latitude = s.Latitude,
        Longitude = s.Longitude,
        Garages = s.Garages.Select(g => new GarageSummaryDto
        {
            Id = g.Id,
            Name = g.Name,
            Size = g.Size,
            TotalSlots = g.TotalSlots,
            AvailableSlots = g.AvailableSlots,
            Position = g.Position,
            LockCode = g.LockCode
        }).ToList()
    };
}
