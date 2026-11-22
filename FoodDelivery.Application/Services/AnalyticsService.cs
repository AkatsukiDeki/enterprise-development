using FoodDelivery.Application.Contracts.Analytics;
using FoodDelivery.Infrastructure.InMemory;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис аналитических запросов службы доставки.
/// </summary>
/// <param name="store">Хранилище данных в памяти.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class AnalyticsService(InMemoryFoodDeliveryStore store, IMapper mapper) : IAnalyticsService
{
    /// <inheritdoc/>
    public Task<IReadOnlyList<int>> GetTop5RestaurantsAsync()
    {
        lock (store.SyncRoot)
        {
            IReadOnlyList<int> result = store.Orders
                .CountBy(o => o.Restaurant.Id)
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key)
                .Take(5)
                .Select(pair => pair.Key)
                .ToList();

            return Task.FromResult(result);
        }
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<int>> GetFastestOrdersAsync()
    {
        lock (store.SyncRoot)
        {
            var deliveredOrders = store.Orders
                .Where(o => o.DeliveryDuration.HasValue)
                .ToList();

            if (deliveredOrders.Count == 0)
            {
                return Task.FromResult<IReadOnlyList<int>>([]);
            }

            var minDuration = deliveredOrders.Min(o => o.DeliveryDuration!.Value);

            IReadOnlyList<int> result = deliveredOrders
                .Where(o => o.DeliveryDuration == minDuration)
                .Select(o => o.Id)
                .ToList();

            return Task.FromResult(result);
        }
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<int>> GetCustomersByRestaurantAsync(int restaurantId)
    {
        lock (store.SyncRoot)
        {
            if (!store.Restaurants.Any(r => r.Id == restaurantId))
            {
                throw new KeyNotFoundException($"Restaurant with Id {restaurantId} not found");
            }

            IReadOnlyList<int> customerIds = store.Orders
                .Where(o => o.Restaurant.Id == restaurantId)
                .Select(o => o.Customer)
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.FullName)
                .Select(c => c.Id)
                .ToList();

            return Task.FromResult(customerIds);
        }
    }

    /// <inheritdoc/>
    public Task<CategorySummaryDto> GetCategorySummaryAsync(
        int categoryId,
        DateTime periodStart,
        DateTime periodEnd)
    {
        if (periodStart > periodEnd)
        {
            throw new ArgumentException("Начало периода не может быть позже окончания периода.");
        }

        lock (store.SyncRoot)
        {
            if (!store.Categories.Any(c => c.Id == categoryId))
            {
                throw new KeyNotFoundException($"Category with Id {categoryId} not found");
            }

            var categoryOrders = store.Orders
                .Where(o => o.CreatedAt >= periodStart && o.CreatedAt <= periodEnd)
                .Where(o => o.Items.Any(i => i.Dish.Category.Id == categoryId))
                .ToList();

            var ordersCount = categoryOrders.Count;

            var totalAmount = categoryOrders
                .SelectMany(o => o.Items)
                .Where(i => i.Dish.Category.Id == categoryId)
                .Sum(i => i.SubTotal);

            var averageAmount = ordersCount > 0 ? Math.Round(totalAmount / ordersCount, 2) : 0m;

            return Task.FromResult(
                mapper.Map<CategorySummaryDto>((categoryId, ordersCount, totalAmount, averageAmount)));
        }
    }

    /// <inheritdoc/>
    public Task<TopCustomerDto> GetTopSpendingCustomerAsync()
    {
        lock (store.SyncRoot)
        {
            var top = store.Orders
                .GroupBy(o => o.Customer.Id)
                .Select(g => (
                    Customer: g.First().Customer,
                    TotalSpent: g.Sum(o => o.TotalAmount)
                ))
                .OrderByDescending(x => x.TotalSpent)
                .FirstOrDefault();

            if (top.Customer is null)
            {
                throw new KeyNotFoundException("No orders found to determine top spending customer");
            }

            return Task.FromResult(mapper.Map<TopCustomerDto>(top));
        }
    }
}
