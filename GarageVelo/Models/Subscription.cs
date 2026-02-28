namespace GarageVelo.Models;

public class Subscription
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string GarageId { get; set; } = string.Empty;
    public PlanType PlanType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= EndDate;
}
