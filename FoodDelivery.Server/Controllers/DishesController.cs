using FoodDelivery.Domain.Entities;
using FoodDelivery.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Controllers;

/// <summary>
/// DTO запроса на создание или обновление блюда.
/// </summary>
public record SaveDishRequest(string Name, int WeightGrams, decimal Price, int CategoryId, int RestaurantId);

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
    public async Task<ActionResult<Dish>> Create([FromBody] SaveDishRequest request, CancellationToken ct)
    {
        if (request.WeightGrams <= 0 || request.Price <= 0)
        {
            return BadRequest("Вес и стоимость блюда должны быть положительными числами.");
        }

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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SaveDishRequest request, CancellationToken ct)
    {
        if (request.WeightGrams <= 0 || request.Price <= 0)
        {
            return BadRequest("Вес и стоимость блюда должны быть положительными числами.");
        }

        var dish = await _db.Dishes
            .Include(d => d.Category)
            .Include(d => d.Restaurant)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        if (dish is null) return NotFound();

        var category = await _db.Categories.FindAsync([request.CategoryId], ct);
        var restaurant = await _db.Restaurants.FindAsync([request.RestaurantId], ct);

        if (category is null || restaurant is null)
        {
            return BadRequest("Указанная категория или ресторан не существуют.");
        }

        dish.Name = request.Name;
        dish.WeightGrams = request.WeightGrams;
        dish.Price = request.Price;
        dish.Category = category;
        dish.Restaurant = restaurant;

        await _db.SaveChangesAsync(ct);
        return NoContent();
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