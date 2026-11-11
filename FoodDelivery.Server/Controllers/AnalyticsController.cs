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
    /// Выбирает только Id и интервал дат, совместим с InMemory и MSSQL.
    /// </summary>
    [HttpGet("fastest-orders")]
    public async Task<ActionResult<IEnumerable<int>>> GetOrdersWithMinDeliveryDuration(CancellationToken ct)
    {
        var deliveredOrders = await _db.Orders
            .Where(o => o.DeliveredAt != null)
            .Select(o => new
            {
                o.Id,
                Duration = o.DeliveredAt!.Value - o.CreatedAt
            })
            .ToListAsync(ct);

        if (deliveredOrders.Count == 0)
        {
            return NotFound("Доставленные заказы отсутствуют.");
        }

        var minDuration = deliveredOrders.Min(o => o.Duration);

        var result = deliveredOrders
            .Where(o => o.Duration == minDuration)
            .Select(o => o.Id)
            .ToList();

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
    /// 4. Сводная информация по категории блюд за указанный период времени.
    /// </summary>
    [HttpGet("category-summary")]
    public async Task<ActionResult<CategorySummaryDto>> GetCategorySummary(
        [FromQuery] int categoryId,
        [FromQuery] DateTime periodStart,
        [FromQuery] DateTime periodEnd,
        CancellationToken ct)
    {
        var baseQuery = _db.Orders
            .Where(o => o.CreatedAt >= periodStart && o.CreatedAt <= periodEnd)
            .Where(o => o.Items.Any(i => i.Dish.Category.Id == categoryId));

        var ordersCount = await baseQuery.CountAsync(ct);

        var totalAmount = ordersCount > 0
            ? await baseQuery
                .SelectMany(o => o.Items)
                .Where(i => i.Dish.Category.Id == categoryId)
                .SumAsync(i => (decimal)i.Quantity * i.Dish.Price, ct)
            : 0m;

        var averageAmount = ordersCount > 0 ? Math.Round(totalAmount / ordersCount, 2) : 0m;

        return Ok(new CategorySummaryDto(categoryId, ordersCount, totalAmount, averageAmount));
    }

    /// <summary>
    /// 5. Клиент, потративший наибольшую сумму за всё время.
    /// </summary>
    [HttpGet("top-spending-customer")]
    public async Task<ActionResult<TopCustomerDto>> GetTopSpendingCustomer(CancellationToken ct)
    {
        var hasOrders = await _db.Orders.AnyAsync(ct);
        if (!hasOrders) return NotFound("Заказы отсутствуют.");

        var top = await _db.Orders
            .GroupBy(o => new
            {
                o.Customer.Id,
                o.Customer.LastName,
                o.Customer.FirstName,
                o.Customer.Patronymic
            })
            .Select(g => new
            {
                CustomerId = g.Key.Id,
                FullName = g.Key.Patronymic != null && g.Key.Patronymic != ""
                    ? g.Key.LastName + " " + g.Key.FirstName + " " + g.Key.Patronymic
                    : g.Key.LastName + " " + g.Key.FirstName,
                TotalSpent = g.Sum(o => o.Items.Sum(i => (decimal)i.Quantity * i.Dish.Price))
            })
            .OrderByDescending(x => x.TotalSpent)
            .FirstOrDefaultAsync(ct);

        if (top is null) return NotFound("Заказы отсутствуют.");

        return Ok(new TopCustomerDto(top.CustomerId, top.FullName, top.TotalSpent));
    }
}