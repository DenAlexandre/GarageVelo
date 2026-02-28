using System.Security.Claims;
using GarageVelo.Api.Data;
using GarageVelo.Api.DTOs;
using GarageVelo.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageVelo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PaymentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var payment = new PaymentEntity
        {
            UserId = userId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            Status = "Completed"
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return Ok(new PaymentResponse
        {
            Id = payment.Id.ToString(),
            Amount = payment.Amount,
            Status = payment.Status,
            PaymentMethod = payment.PaymentMethod,
            CreatedAt = payment.CreatedAt
        });
    }
}
