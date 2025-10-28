namespace NuaSpa.Core.Entities;

public class Appointment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int StaffId { get; set; }
    public int ServiceId { get; set; }
    public int RoomId { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Status { get; set; } = "Pending"; // Pending / Confirmed / Cancelled

    public Client Client { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public Room Room { get; set; } = null!;
}
