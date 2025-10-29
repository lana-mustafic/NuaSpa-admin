using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuaSpa.Api.Models;
using NuaSpa.Core.Entities;
using NuaSpa.Infrastructure.Data;

namespace NuaSpa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // staff & clients can view, admin full control
public class AppointmentsController : ControllerBase
{
    private readonly NuaSpaDbContext _db;

    public AppointmentsController(NuaSpaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
    {
        var list = await _db.Appointments
            .Include(a => a.Client).ThenInclude(c => c.User)
            .Include(a => a.Staff).ThenInclude(s => s.User)
            .Include(a => a.Service)
            .Include(a => a.Room)
            .AsNoTracking()
            .ToListAsync();

        return Ok(list.Select(a => new AppointmentDto(
            a.Id,
            a.ClientId,
            a.Client.User.FullName,
            a.StaffId,
            a.Staff.User.FullName,
            a.ServiceId,
            a.Service.Name,
            a.RoomId,
            a.Room.Name,
            a.Start,
            a.End,
            a.Status
        )));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult> Create(CreateAppointmentDto dto)
    {
        var service = await _db.Services.FindAsync(dto.ServiceId);
        if (service is null) return BadRequest("Service not found.");

        var start = dto.Start;
        var end = start.AddMinutes(service.DurationMinutes);

        // 1) Check if appointment fits inside staff working hours
        var day = (int)start.DayOfWeek;
        if (day == 0) day = 7; // Sunday fix (C# uses 0 for Sunday)

        var workingHours = await _db.WorkingHours
            .Where(w => w.StaffId == dto.StaffId && w.DayOfWeek == day)
            .FirstOrDefaultAsync();

        if (workingHours is null)
            return BadRequest("Staff does not work this day.");

        if (start.TimeOfDay < workingHours.Start || end.TimeOfDay > workingHours.End)
            return BadRequest("Appointment is outside of staff working hours.");

        // 2) Check staff overlap
        var staffOverlap = await _db.Appointments.AnyAsync(a =>
            a.StaffId == dto.StaffId &&
            a.Start < end &&
            start < a.End
        );

        if (staffOverlap)
            return BadRequest("Staff is already booked for this time.");

        // 3) Check room overlap
        var roomOverlap = await _db.Appointments.AnyAsync(a =>
            a.RoomId == dto.RoomId &&
            a.Start < end &&
            start < a.End
        );

        if (roomOverlap)
            return BadRequest("Room is already in use during this time.");

        // Everything valid → create appointment
        var appt = new Appointment
        {
            ClientId = dto.ClientId,
            StaffId = dto.StaffId,
            ServiceId = dto.ServiceId,
            RoomId = dto.RoomId,
            Start = start,
            End = end,
            Status = "Scheduled"
        };

        _db.Appointments.Add(appt);
        await _db.SaveChangesAsync();
        return Ok();
    }


    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<ActionResult> UpdateStatus(int id, UpdateAppointmentStatusDto dto)
    {
        var entity = await _db.Appointments.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Status = dto.Status;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var entity = await _db.Appointments.FindAsync(id);
        if (entity is null) return NotFound();

        _db.Appointments.Remove(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
