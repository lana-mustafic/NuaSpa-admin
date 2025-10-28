namespace NuaSpa.Core.Entities;

public class Client
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? SkinType { get; set; }
    public string? Allergies { get; set; }
    public string? Notes { get; set; }

    public User User { get; set; } = null!;
}
