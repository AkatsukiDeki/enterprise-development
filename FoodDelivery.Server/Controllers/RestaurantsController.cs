using FoodDelivery.Domain.Entities;
using FoodDelivery.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly FoodDeliveryDbContext _db;

    public RestaurantsController(FoodDeliveryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Restaurant>>> GetAll(CancellationToken ct)
    {
        return await _db.Restaurants.AsNoTracking().ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Restaurant>> GetById(int id, CancellationToken ct)
    {
        var restaurant = await _db.Restaurants.FindAsync([id], ct);
        return restaurant is null ? NotFound() : restaurant;
    }

    /// <summary>
    /// Получение коллекции ассоциированных блюд ресторана.
    /// </summary>
    [HttpGet("{id:int}/dishes")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetDishes(int id, CancellationToken ct)
    {
        var exists = await _db.Restaurants.AnyAsync(r => r.Id == id, ct);
        if (!exists) return NotFound();

        return await _db.Dishes
            .AsNoTracking()
            .Where(d => d.Restaurant.Id == id)
            .Include(d => d.Category)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Получение коллекции ассоциированных заказов ресторана.
    /// </summary>
    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int id, CancellationToken ct)
    {
        var exists = await _db.Restaurants.AnyAsync(r => r.Id == id, ct);
        if (!exists) return NotFound();

        return await _db.Orders
            .AsNoTracking()
            .Where(o => o.Restaurant.Id == id)
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
            .ToListAsync(ct);
    }

    [HttpPost]
    public async Task<ActionResult<Restaurant>> Create([FromBody] Restaurant restaurant, CancellationToken ct)
    {
        _db.Restaurants.Add(restaurant);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, restaurant);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Restaurant updated, CancellationToken ct)
    {
        var restaurant = await _db.Restaurants.FindAsync([id], ct);
        if (restaurant is null) return NotFound();

        restaurant.Name = updated.Name;
        restaurant.Address = updated.Address;
        restaurant.Rating = updated.Rating;
        restaurant.OpeningTime = updated.OpeningTime;
        restaurant.ClosingTime = updated.ClosingTime;

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var restaurant = await _db.Restaurants.FindAsync([id], ct);
        if (restaurant is null) return NotFound();

        _db.Restaurants.Remove(restaurant);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}