using GarageVelo.Models;

namespace GarageVelo.Services;

public interface ISubscriptionService
{
    Task<List<SubscriptionPlan>> GetPlansAsync();
    Task<Subscription?> ActivateAsync(string userId, string garageId, PlanType planType);
    Task<Subscription?> GetActiveSubscriptionAsync(string userId);
    Task<bool> CancelAsync(string subscriptionId);
}
