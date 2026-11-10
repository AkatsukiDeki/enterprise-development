using FoodDelivery.Domain.Entities;
using FoodDelivery.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Controllers;

public record CreateDishRequest(string Name, int WeightGrams, decimal Price, int CategoryId, int RestaurantId);

[ApiController]
[Route("api/[controller]")]
public class DishesController : ControllerBase
{
    private readonly FoodDeliveryDbContext _db;

    public DishesController(FoodDeliveryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dish>>> GetAll(CancellationToken ct)
    {
        return await _db.Dishes
            .Include(d => d.Category)
            .Include(d => d.Restaurant)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Dish>> GetById(int id, CancellationToken ct)
    {
        var dish = await _db.Dishes
            .Include(d => d.Category)
            .Include(d => d.Restaurant)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        return dish is null ? NotFound() : dish;
    }

    [HttpPost]
    public async Task<ActionResult<Dish>> Create([FromBody] CreateDishRequest request, CancellationToken ct)
    {
        var category = await _db.Categories.FindAsync([request.CategoryId], ct);
        var restaurant = await _db.Restaurants.FindAsync([request.RestaurantId], ct);

        if (category is null || restaurant is null)
        {
            return BadRequest("Указанная категория или ресторан не существуют.");
        }

        var dish = new Dish
        {
            Id = 0,
            Name = request.Name,
            WeightGrams = request.WeightGrams,
            Price = request.Price,
            Category = category,
            Restaurant = restaurant
        };

        _db.Dishes.Add(dish);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dish);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var dish = await _db.Dishes.FindAsync([id], ct);
        if (dish is null) return NotFound();

        _db.Dishes.Remove(dish);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}