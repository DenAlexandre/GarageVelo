using System.ComponentModel.DataAnnotations;

namespace GarageVelo.Api.Entities;

public class UserEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SubscriptionEntity> Subscriptions { get; set; } = [];
    public ICollection<PaymentEntity> Payments { get; set; } = [];
    public ICollection<LoginSessionEntity> LoginSessions { get; set; } = [];
}
