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