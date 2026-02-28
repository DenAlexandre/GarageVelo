namespace GarageVelo.Models;

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed
}

public class Payment
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string PaymentMethod { get; set; } = "Card";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
