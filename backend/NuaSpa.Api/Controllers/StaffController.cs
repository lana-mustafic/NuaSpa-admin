using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NuaSpa.Api.Models;
using NuaSpa.Infrastructure.Data;
using NuaSpa.Core.Entities;
using BCrypt.Net;

namespace NuaSpa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class StaffController : ControllerBase
{
    private readonly NuaSpaDbContext _db;

    public StaffController(NuaSpaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
    {
        var staff = await _db.Staff
            .Include(s => s.User)
            .AsNoTracking()
            .Select(s => new StaffDto(
                s.Id,
                s.User.FullName,
                s.User.PhoneNumber,
                s.User.Email,
                s.Bio,
                s.Specialties
            ))
            .ToListAsync();

        return Ok(staff);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateStaffDto dto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = "Staff"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var staff = new Staff
        {
            UserId = user.Id,
            Bio = dto.Bio,
            Specialties = dto.Specialties
        };

        _db.Staff.Add(staff);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, UpdateStaffDto dto)
    {
        var staff = await _db.Staff.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        if (staff is null) return NotFound();

        staff.User.FullName = dto.FullName;
        staff.User.PhoneNumber = dto.PhoneNumber;
        staff.Bio = dto.Bio;
        staff.Specialties = dto.Specialties;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var staff = await _db.Staff.FindAsync(id);
        if (staff is null) return NotFound();

        var user = await _db.Users.FindAsync(staff.UserId);

        _db.Staff.Remove(staff);
        if (user != null) _db.Users.Remove(user);

        await _db.SaveChangesAsync();
        return NoContent();
    }
}
