using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuaSpa.Api.Models;
using NuaSpa.Infrastructure.Data;
using NuaSpa.Core.Entities;

namespace NuaSpa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class WorkingHoursController : ControllerBase
{
    private readonly NuaSpaDbContext _db;

    public WorkingHoursController(NuaSpaDbContext db)
    {
        _db = db;
    }

    [HttpGet("{staffId:int}")]
    public async Task<ActionResult<IEnumerable<WorkingHoursDto>>> GetByStaff(int staffId)
    {
        var hours = await _db.WorkingHours
            .Include(w => w.Staff).ThenInclude(s => s.User)
            .Where(w => w.StaffId == staffId)
            .AsNoTracking()
            .ToListAsync();

        return Ok(hours.Select(w => new WorkingHoursDto(
            w.Id,
            w.StaffId,
            w.Staff.User.FullName,
            w.DayOfWeek,
            w.Start.ToString(@"hh\:mm"),
            w.End.ToString(@"hh\:mm")
        )));
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateWorkingHoursDto dto)
    {
        var staffExists = await _db.Staff.AnyAsync(s => s.Id == dto.StaffId);
        if (!staffExists) return BadRequest("Staff does not exist.");

        var entity = new WorkingHours
        {
            StaffId = dto.StaffId,
            DayOfWeek = dto.DayOfWeek,
            Start = TimeSpan.Parse(dto.Start),
            End = TimeSpan.Parse(dto.End)
        };

        _db.WorkingHours.Add(entity);
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateWorkingHoursDto dto)
    {
        var entity = await _db.WorkingHours.FindAsync(id);
        if (entity is null) return NotFound();

        entity.DayOfWeek = dto.DayOfWeek;
        entity.Start = TimeSpan.Parse(dto.Start);
        entity.End = TimeSpan.Parse(dto.End);

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var entity = await _db.WorkingHours.FindAsync(id);
        if (entity is null) return NotFound();

        _db.WorkingHours.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
