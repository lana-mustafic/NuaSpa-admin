namespace NuaSpa.Api.Models;

public record AppointmentDto(
    int Id,
    int ClientId,
    string ClientName,
    int StaffId,
    string StaffName,
    int ServiceId,
    string ServiceName,
    int RoomId,
    string RoomName,
    DateTime Start,
    DateTime End,
    string Status
);

public record CreateAppointmentDto(
    int ClientId,
    int StaffId,
    int ServiceId,
    int RoomId,
    DateTime Start
);

public record UpdateAppointmentStatusDto(
    string Status
);
