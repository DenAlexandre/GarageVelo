using GarageVelo.Models;

namespace GarageVelo.Services;

public interface IPaymentService
{
    Task<Payment> ProcessPaymentAsync(string userId, decimal amount, string paymentMethod = "Card");
}
