using FoodDelivery.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Controllers;

public record CategorySummaryDto(int CategoryId, int OrdersCount, decimal TotalAmount, decimal AverageAmount);
public record TopCustomerDto(int CustomerId, string FullName, decimal TotalSpent);

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly FoodDeliveryDbContext _db;

    public AnalyticsController(FoodDeliveryDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// 1. Топ-5 ресторанов по количеству заказов.
    /// </summary>
    [HttpGet("top-restaurants")]
    public async Task<ActionResult<IEnumerable<int>>> GetTop5Restaurants(CancellationToken ct)
    {
        var top = await _db.Orders
            .GroupBy(o => o.Restaurant.Id)
            .Select(g => new { RestaurantId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.RestaurantId)
            .Take(5)
            .Select(x => x.RestaurantId)
            .ToListAsync(ct);

        return Ok(top);
    }

    /// <summary>
    /// 2. Идентификаторы заказов с минимальным временем доставки.
    /// </summary>
    [HttpGet("fastest-orders")]
    public async Task<ActionResult> GetOrdersWithMinDeliveryDuration(CancellationToken ct)
    {
        var deliveredOrders = await _db.Orders
            .Where(o => o.DeliveredAt.HasValue)
            .Select(o => new
            {
                o.Id,
                DurationMinutes = EF.Functions.DateDiffMinute(o.CreatedAt, o.DeliveredAt!.Value)
            })
            .ToListAsync(ct);

        if (deliveredOrders.Count == 0)
        {
            return NotFound("Доставленные заказы отсутствуют.");
        }

        var minDuration = deliveredOrders.Min(o => o.DurationMinutes);
        var result = deliveredOrders.Where(o => o.DurationMinutes == minDuration).Select(o => o.Id);

        return Ok(result);
    }

    /// <summary>
    /// 3. Идентификаторы клиентов, заказывавших в указанном ресторане, отсортированные по ФИО.
    /// </summary>
    [HttpGet("restaurant-customers/{restaurantId:int}")]
    public async Task<ActionResult<IEnumerable<int>>> GetCustomersByRestaurant(int restaurantId, CancellationToken ct)
    {
        var customerIds = await _db.Orders
            .Where(o => o.Restaurant.Id == restaurantId)
            .Select(o => o.Customer)
            .Distinct()
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ThenBy(c => c.Patronymic)
            .Select(c => c.Id)
            .ToListAsync(ct);

        return Ok(customerIds);
    }

    /// <summary>
    /// 4. Сводная информация по категории блюд за указанный период времени (с фильтрацией границ дат).
    /// </summary>
    [HttpGet("category-summary")]
    public async Task<ActionResult<CategorySummaryDto>> GetCategorySummary(
        [FromQuery] int categoryId,
        [FromQuery] DateTime periodStart,
        [FromQuery] DateTime periodEnd,
        CancellationToken ct)
    {
        var categoryOrders = await _db.Orders
            .Where(o => o.CreatedAt >= periodStart && o.CreatedAt <= periodEnd)
            .Where(o => o.Items.Any(item => item.Dish.Category.Id == categoryId))
            .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
            .ToListAsync(ct);

        var ordersCount = categoryOrders.Count;
        var totalAmount = categoryOrders
            .SelectMany(o => o.Items)
            .Where(i => i.Dish.Category.Id == categoryId)
            .Sum(i => i.Quantity * i.Dish.Price);

        var averageAmount = ordersCount > 0 ? Math.Round(totalAmount / ordersCount, 2) : 0m;

        return Ok(new CategorySummaryDto(categoryId, ordersCount, totalAmount, averageAmount));
    }

    /// <summary>
    /// 5. Клиент, потративший наибольшую сумму за всё время.
    /// </summary>
    [HttpGet("top-spending-customer")]
    public async Task<ActionResult<TopCustomerDto>> GetTopSpendingCustomer(CancellationToken ct)
    {
        var orders = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
            .ToListAsync(ct);

        if (orders.Count == 0) return NotFound("Заказы отсутствуют.");

        var top = orders
            .GroupBy(o => o.Customer)
            .Select(g => new TopCustomerDto(
                g.Key.Id,
                g.Key.FullName,
                g.Sum(o => o.Items.Sum(i => i.Quantity * i.Dish.Price))
            ))
            .OrderByDescending(x => x.TotalSpent)
            .FirstOrDefault();

        return Ok(top);
    }
}