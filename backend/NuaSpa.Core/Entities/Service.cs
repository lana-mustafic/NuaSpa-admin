namespace NuaSpa.Core.Entities;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }

    public ServiceCategory Category { get; set; } = null!;
}
