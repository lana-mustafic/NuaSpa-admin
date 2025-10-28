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
public class ServiceCategoriesController : ControllerBase
{
    private readonly NuaSpaDbContext _db;
    private readonly IMapper _mapper;

    public ServiceCategoriesController(NuaSpaDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous] // mobile can read categories without login if you want
    public async Task<ActionResult<IEnumerable<ServiceCategoryDto>>> GetAll()
    {
        var items = await _db.ServiceCategories.AsNoTracking().ToListAsync();
        return Ok(_mapper.Map<IEnumerable<ServiceCategoryDto>>(items));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceCategoryDto>> GetById(int id)
    {
        var item = await _db.ServiceCategories.FindAsync(id);
        return item is null ? NotFound() : Ok(_mapper.Map<ServiceCategoryDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceCategoryDto>> Create(CreateServiceCategoryDto dto)
    {
        var entity = _mapper.Map<ServiceCategory>(dto);
        _db.ServiceCategories.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, _mapper.Map<ServiceCategoryDto>(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateServiceCategoryDto dto)
    {
        var entity = await _db.ServiceCategories.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.ServiceCategories.FindAsync(id);
        if (entity is null) return NotFound();
        _db.ServiceCategories.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
