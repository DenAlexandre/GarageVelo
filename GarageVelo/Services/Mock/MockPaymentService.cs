using GarageVelo.Models;

namespace GarageVelo.Services.Mock;

public class MockPaymentService : IPaymentService
{
    public async Task<Payment> ProcessPaymentAsync(string userId, decimal amount, string paymentMethod = "Card")
    {
        // Simulate 2-second payment processing
        await Task.Delay(2000);

        return new Payment
        {
            Id = $"PAY-{Guid.NewGuid():N}"[..12],
            UserId = userId,
            Amount = amount,
            Status = PaymentStatus.Completed,
            PaymentMethod = paymentMethod,
            CreatedAt = DateTime.Now
        };
    }
}
