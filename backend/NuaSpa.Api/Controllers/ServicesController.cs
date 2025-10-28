using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuaSpa.Api.Models;
using NuaSpa.Infrastructure.Data;
using NuaSpa.Core.Entities;

namespace NuaSpa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Staff")]
public class ServicesController : ControllerBase
{
    private readonly NuaSpaDbContext _db;
    private readonly IMapper _mapper;

    public ServicesController(NuaSpaDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous] // optional: allow mobile catalog without auth
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll([FromQuery] int? categoryId)
    {
        var query = _db.Services.AsNoTracking().AsQueryable();
        if (categoryId.HasValue) query = query.Where(s => s.CategoryId == categoryId.Value);
        var items = await query.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<ServiceDto>>(items));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceDto>> GetById(int id)
    {
        var item = await _db.Services.FindAsync(id);
        return item is null ? NotFound() : Ok(_mapper.Map<ServiceDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create(CreateServiceDto dto)
    {
        // basic guard: category must exist
        var catExists = await _db.ServiceCategories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!catExists) return BadRequest("Category not found.");

        var entity = _mapper.Map<Service>(dto);
        _db.Services.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, _mapper.Map<ServiceDto>(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateServiceDto dto)
    {
        var entity = await _db.Services.FindAsync(id);
        if (entity is null) return NotFound();

        // guard category
        var catExists = await _db.ServiceCategories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!catExists) return BadRequest("Category not found.");

        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Services.FindAsync(id);
        if (entity is null) return NotFound();

        _db.Services.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
