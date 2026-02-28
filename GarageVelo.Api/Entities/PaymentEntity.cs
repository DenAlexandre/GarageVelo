using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageVelo.Api.Entities;

public class PaymentEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending";

    [Required, MaxLength(50)]
    public string PaymentMethod { get; set; } = "Card";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
