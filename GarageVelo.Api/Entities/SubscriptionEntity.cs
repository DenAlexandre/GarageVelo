using System.ComponentModel.DataAnnotations;

namespace GarageVelo.Api.Entities;

public class SubscriptionEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    [MaxLength(10)]
    public string GarageId { get; set; } = string.Empty;
    public GarageEntity Garage { get; set; } = null!;

    [Required, MaxLength(20)]
    public string PlanType { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
