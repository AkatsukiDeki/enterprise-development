using FoodDelivery.Domain.Entities;
using FoodDelivery.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly FoodDeliveryDbContext _db;

    public CategoriesController(FoodDeliveryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll(CancellationToken ct)
    {
        return await _db.Categories.AsNoTracking().ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id, CancellationToken ct)
    {
        var category = await _db.Categories.FindAsync([id], ct);
        return category is null ? NotFound() : category;
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create([FromBody] Category category, CancellationToken ct)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category updateDto, CancellationToken ct)
    {
        var category = await _db.Categories.FindAsync([id], ct);
        if (category is null) return NotFound();

        category.Name = updateDto.Name;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var category = await _db.Categories.FindAsync([id], ct);
        if (category is null) return NotFound();

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}