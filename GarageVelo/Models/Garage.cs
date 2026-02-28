namespace GarageVelo.Models;

public class Garage
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Size { get; set; } = "Medium"; // Small, Medium, Large
    public int TotalSlots { get; set; }
    public int AvailableSlots { get; set; }
    public int Position { get; set; }
    public string LockCode { get; set; } = string.Empty;
}
