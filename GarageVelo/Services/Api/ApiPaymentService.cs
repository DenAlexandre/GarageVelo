using System.Net.Http.Json;
using GarageVelo.Models;

namespace GarageVelo.Services.Api;

public class ApiPaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public ApiPaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Payment> ProcessPaymentAsync(string userId, decimal amount, string paymentMethod = "Card")
    {
        var response = await _httpClient.PostAsJsonAsync("api/payments", new
        {
            amount,
            paymentMethod
        });

        if (!response.IsSuccessStatusCode)
        {
            return new Payment
            {
                Id = Guid.NewGuid().ToString("N")[..12],
                UserId = userId,
                Amount = amount,
                Status = PaymentStatus.Failed,
                PaymentMethod = paymentMethod
            };
        }

        var result = await response.Content.ReadFromJsonAsync<PaymentResponse>();
        return new Payment
        {
            Id = result?.Id ?? Guid.NewGuid().ToString("N")[..12],
            UserId = userId,
            Amount = amount,
            Status = result?.Status == "Completed" ? PaymentStatus.Completed : PaymentStatus.Failed,
            PaymentMethod = paymentMethod,
            CreatedAt = result?.CreatedAt ?? DateTime.Now
        };
    }

    private class PaymentResponse
    {
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
