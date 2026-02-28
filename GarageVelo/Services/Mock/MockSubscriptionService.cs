using GarageVelo.Models;

namespace GarageVelo.Services.Mock;

public class MockSubscriptionService : ISubscriptionService
{
    private readonly List<Subscription> _subscriptions = [];

    public Task<List<SubscriptionPlan>> GetPlansAsync()
    {
        return Task.FromResult(SubscriptionPlan.GetPlans());
    }

    public async Task<Subscription?> ActivateAsync(string userId, string garageId, PlanType planType)
    {
        await Task.Delay(500);

        var plan = SubscriptionPlan.GetPlans().First(p => p.Type == planType);
        var subscription = new Subscription
        {
            Id = $"SUB-{_subscriptions.Count + 1:D4}",
            UserId = userId,
            GarageId = garageId,
            PlanType = planType,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(plan.DurationDays)
        };

        _subscriptions.Add(subscription);
        return subscription;
    }

    public Task<Subscription?> GetActiveSubscriptionAsync(string userId)
    {
        var sub = _subscriptions
            .Where(s => s.UserId == userId && s.IsActive)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();

        return Task.FromResult(sub);
    }

    public Task<bool> CancelAsync(string subscriptionId)
    {
        var sub = _subscriptions.FirstOrDefault(s => s.Id == subscriptionId);
        if (sub is null) return Task.FromResult(false);

        _subscriptions.Remove(sub);
        return Task.FromResult(true);
    }
}
