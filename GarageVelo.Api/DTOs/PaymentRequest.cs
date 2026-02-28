using System.ComponentModel.DataAnnotations;

namespace GarageVelo.Api.DTOs;

public class PaymentRequest
{
    [Required]
    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = "Card";
}

public class PaymentResponse
{
    public string Id { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
