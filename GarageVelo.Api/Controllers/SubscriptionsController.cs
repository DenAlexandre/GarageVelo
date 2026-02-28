using System.Security.Claims;
using GarageVelo.Api.Data;
using GarageVelo.Api.DTOs;
using GarageVelo.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageVelo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public SubscriptionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetPlans()
    {
        var plans = await _db.SubscriptionPlans.ToListAsync();
        return Ok(plans.Select(p => new SubscriptionPlanDto
        {
            Id = p.Id.ToString(),
            PlanType = p.PlanType,
            Name = p.Name,
            Price = p.Price,
            DurationDays = p.DurationDays
        }));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Subscribe([FromBody] CreateSubscriptionRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var garage = await _db.Garages.FindAsync(request.GarageId);
        if (garage == null)
            return NotFound(new { message = "Garage non trouvé." });

        var plan = await _db.SubscriptionPlans.FirstOrDefaultAsync(p => p.PlanType == request.PlanType);
        if (plan == null)
            return BadRequest(new { message = "Plan d'abonnement invalide." });

        var subscription = new SubscriptionEntity
        {
            UserId = userId,
            GarageId = request.GarageId,
            PlanType = request.PlanType,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(plan.DurationDays)
        };

        _db.Subscriptions.Add(subscription);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMySubscription), MapDto(subscription));
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMySubscription()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var now = DateTime.UtcNow;

        var subscription = await _db.Subscriptions
            .Where(s => s.UserId == userId && s.StartDate <= now && s.EndDate >= now)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync();

        if (subscription == null)
            return Ok((SubscriptionDto?)null);

        return Ok(MapDto(subscription));
    }

    private static SubscriptionDto MapDto(SubscriptionEntity s) => new()
    {
        Id = s.Id.ToString(),
        UserId = s.UserId.ToString(),
        GarageId = s.GarageId,
        PlanType = s.PlanType,
        StartDate = s.StartDate,
        EndDate = s.EndDate,
        IsActive = DateTime.UtcNow >= s.StartDate && DateTime.UtcNow <= s.EndDate
    };
}
