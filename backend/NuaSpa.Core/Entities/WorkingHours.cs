namespace NuaSpa.Core.Entities;

public class WorkingHours
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public int DayOfWeek { get; set; } // 1=Mon, 7=Sun
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }

    public Staff Staff { get; set; } = null!;
}
