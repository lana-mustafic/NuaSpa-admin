namespace NuaSpa.Api.Models;

public record WorkingHoursDto(
    int Id,
    int StaffId,
    string StaffName,
    int DayOfWeek,
    string Start,
    string End
);

public record CreateWorkingHoursDto(
    int StaffId,
    int DayOfWeek,
    string Start,
    string End
);

public record UpdateWorkingHoursDto(
    int DayOfWeek,
    string Start,
    string End
);
