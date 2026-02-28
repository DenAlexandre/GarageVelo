using System.ComponentModel.DataAnnotations;

namespace GarageVelo.Api.Entities;

public class SiteEntity
{
    [Key, MaxLength(10)]
    public string Id { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Address { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public ICollection<GarageEntity> Garages { get; set; } = [];
}
