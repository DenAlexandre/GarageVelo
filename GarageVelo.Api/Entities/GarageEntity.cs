using System.ComponentModel.DataAnnotations;

namespace GarageVelo.Api.Entities;

public class GarageEntity
{
    [Key, MaxLength(10)]
    public string Id { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string SiteId { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Size { get; set; } = "Medium";

    public int TotalSlots { get; set; }
    public int AvailableSlots { get; set; }
    public int Position { get; set; }

    [Required, MaxLength(20)]
    public string LockCode { get; set; } = string.Empty;

    public SiteEntity Site { get; set; } = null!;
    public ICollection<SubscriptionEntity> Subscriptions { get; set; } = [];
}
