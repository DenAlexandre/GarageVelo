using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageVelo.Api.Entities;

public class SubscriptionPlanEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(20)]
    public string PlanType { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int DurationDays { get; set; }
}
