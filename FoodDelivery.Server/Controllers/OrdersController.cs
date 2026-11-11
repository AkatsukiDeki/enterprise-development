using FoodDelivery.Domain.Entities;
using FoodDelivery.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Controllers;

public record CreateOrderItemRequest(int DishId, int Quantity);
public record CreateOrderRequest(int CustomerId, int RestaurantId, List<CreateOrderItemRequest> Items);

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly FoodDeliveryDbContext _db;

    public OrdersController(FoodDeliveryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll(CancellationToken ct)
    {
        return await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Restaurant)
            .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
                    .ThenInclude(d => d.Category)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetById(int id, CancellationToken ct)
    {
        var order = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Restaurant)
            .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
                    .ThenInclude(d => d.Category)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

        return order is null ? NotFound() : order;
    }

    /// <summary>
    /// Получение коллекции ассоциированных позиций заказа.
    /// </summary>
    [HttpGet("{id:int}/items")]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(int id, CancellationToken ct)
    {
        var exists = await _db.Orders.AnyAsync(o => o.Id == id, ct);
        if (!exists) return NotFound();

        return await _db.Orders
            .Where(o => o.Id == id)
            .SelectMany(o => o.Items)
            .Include(i => i.Dish)
                .ThenInclude(d => d.Category)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var customer = await _db.Customers.FindAsync([request.CustomerId], ct);
        var restaurant = await _db.Restaurants.FindAsync([request.RestaurantId], ct);

        if (customer is null || restaurant is null)
        {
            return BadRequest("Клиент или ресторан не найдены.");
        }

        var dishIds = request.Items.Select(i => i.DishId).Distinct().ToList();
        var dishes = await _db.Dishes
            .Include(d => d.Restaurant)
            .Where(d => dishIds.Contains(d.Id))
            .ToDictionaryAsync(d => d.Id, ct);

        // Инвариант: все выбранные блюда должны принадлежать указанному ресторану
        foreach (var item in request.Items)
        {
            if (!dishes.TryGetValue(item.DishId, out var dish))
            {
                return BadRequest($"Блюдо с ID {item.DishId} не найдено.");
            }

            if (dish.Restaurant.Id != restaurant.Id)
            {
                return BadRequest($"Блюдо '{dish.Name}' не принадлежит ресторану '{restaurant.Name}'.");
            }
        }

        var order = new Order
        {
            Id = 0,
            Customer = customer,
            Restaurant = restaurant,
            CreatedAt = DateTime.UtcNow,
            DeliveredAt = null,
            Items = request.Items.Select(i => new OrderItem
            {
                Id = 0,
                Dish = dishes[i.DishId],
                Quantity = i.Quantity
            }).ToList()
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

        if (order is null) return NotFound();

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}