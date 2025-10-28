namespace NuaSpa.Core.Entities;

public class Review
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }

    public Appointment Appointment { get; set; } = null!;
}
