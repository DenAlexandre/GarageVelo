using System.ComponentModel.DataAnnotations;

namespace GarageVelo.Api.DTOs;

public class SubscriptionDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string GarageId { get; set; } = string.Empty;
    public string PlanType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}

public class SubscriptionPlanDto
{
    public string Id { get; set; } = string.Empty;
    public string PlanType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
}

public class CreateSubscriptionRequest
{
    [Required]
    public string GarageId { get; set; } = string.Empty;

    [Required]
    public string PlanType { get; set; } = string.Empty;
}
