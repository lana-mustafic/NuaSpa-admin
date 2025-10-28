namespace NuaSpa.Core.Entities;

public class Staff
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string Specialties { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}
