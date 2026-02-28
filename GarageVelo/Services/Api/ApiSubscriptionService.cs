using System.Net.Http.Json;
using GarageVelo.Models;

namespace GarageVelo.Services.Api;

public class ApiSubscriptionService : ISubscriptionService
{
    private readonly HttpClient _httpClient;

    public ApiSubscriptionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SubscriptionPlan>> GetPlansAsync()
    {
        var plans = await _httpClient.GetFromJsonAsync<List<PlanResponse>>("api/subscriptions/plans");
        if (plans == null)
            return SubscriptionPlan.GetPlans();

        return plans.Select(p => new SubscriptionPlan
        {
            Type = Enum.Parse<PlanType>(p.PlanType),
            Name = p.Name,
            Price = p.Price,
            DurationDays = p.DurationDays,
            Description = p.PlanType switch
            {
                "Daily" => "Accès 24h",
                "Monthly" => "Accès 30 jours",
                "Yearly" => "Accès 1 an",
                _ => ""
            }
        }).ToList();
    }

    public async Task<Subscription?> ActivateAsync(string userId, string garageId, PlanType planType)
    {
        var response = await _httpClient.PostAsJsonAsync("api/subscriptions", new
        {
            garageId,
            planType = planType.ToString()
        });

        if (!response.IsSuccessStatusCode)
            return null;

        var dto = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
        if (dto == null)
            return null;

        return new Subscription
        {
            Id = dto.Id,
            UserId = dto.UserId,
            GarageId = dto.GarageId,
            PlanType = Enum.Parse<PlanType>(dto.PlanType),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }

    public async Task<Subscription?> GetActiveSubscriptionAsync(string userId)
    {
        var response = await _httpClient.GetAsync("api/subscriptions/my");
        if (!response.IsSuccessStatusCode)
            return null;

        var dto = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
        if (dto == null || string.IsNullOrEmpty(dto.Id))
            return null;

        return new Subscription
        {
            Id = dto.Id,
            UserId = dto.UserId,
            GarageId = dto.GarageId,
            PlanType = Enum.Parse<PlanType>(dto.PlanType),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }

    public Task<bool> CancelAsync(string subscriptionId)
    {
        // Not yet implemented on the API side
        return Task.FromResult(false);
    }

    private class PlanResponse
    {
        public string Id { get; set; } = string.Empty;
        public string PlanType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
    }

    private class SubscriptionResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string GarageId { get; set; } = string.Empty;
        public string PlanType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
