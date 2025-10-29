namespace NuaSpa.Api.Models;

public record StaffDto(
    int Id,
    string FullName,
    string PhoneNumber,
    string Email,
    string Bio,
    string Specialties
);

public record CreateStaffDto(
    string FullName,
    string PhoneNumber,
    string Email,
    string Password,
    string Bio,
    string Specialties
);

public record UpdateStaffDto(
    string FullName,
    string PhoneNumber,
    string Bio,
    string Specialties
);
