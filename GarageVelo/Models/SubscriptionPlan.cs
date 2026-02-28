namespace GarageVelo.Models;

public enum PlanType
{
    Daily,
    Monthly,
    Yearly
}

public class SubscriptionPlan
{
    public PlanType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string Description { get; set; } = string.Empty;

    public static List<SubscriptionPlan> GetPlans() =>
    [
        new() { Type = PlanType.Daily,   Name = "Jour",  Price = 1m,   DurationDays = 1,   Description = "Accès 24h" },
        new() { Type = PlanType.Monthly, Name = "Mois",  Price = 20m,  DurationDays = 30,  Description = "Accès 30 jours" },
        new() { Type = PlanType.Yearly,  Name = "Année", Price = 200m, DurationDays = 365, Description = "Accès 1 an" }
    ];
}
