using FoodDelivery.Application.Contracts.Analytics;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис аналитических запросов службы доставки.
/// </summary>
/// <param name="store">Хранилище данных.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class AnalyticsService(IFoodDeliveryStore store, IMapper mapper) : IAnalyticsService
{
    /// <inheritdoc/>
    public IReadOnlyList<int> GetTop5Restaurants()
    {
        lock (store.SyncRoot)
        {
            return store.Orders
                .CountBy(o => o.Restaurant.Id)
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key)
                .Take(5)
                .Select(pair => pair.Key)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<int> GetFastestOrders()
    {
        lock (store.SyncRoot)
        {
            TimeSpan? minDuration = null;
            var fastestOrderIds = new List<int>();

            foreach (var order in store.Orders)
            {
                if (!order.DeliveryDuration.HasValue)
                {
                    continue;
                }

                var duration = order.DeliveryDuration.Value;
                if (!minDuration.HasValue || duration < minDuration.Value)
                {
                    minDuration = duration;
                    fastestOrderIds.Clear();
                    fastestOrderIds.Add(order.Id);
                }
                else if (duration == minDuration.Value)
                {
                    fastestOrderIds.Add(order.Id);
                }
            }

            return fastestOrderIds;
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<int> GetCustomersByRestaurant(int restaurantId)
    {
        lock (store.SyncRoot)
        {
            if (!store.Restaurants.Any(r => r.Id == restaurantId))
            {
                throw new KeyNotFoundException($"Restaurant with Id {restaurantId} not found");
            }

            return store.Orders
                .Where(o => o.Restaurant.Id == restaurantId)
                .Select(o => o.Customer)
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.FullName)
                .Select(c => c.Id)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public CategorySummaryDto GetCategorySummary(
        int categoryId,
        DateTime periodStart,
        DateTime periodEnd)
    {
        if (periodStart > periodEnd)
        {
            throw new ArgumentException("Period start cannot be later than period end.");
        }

        lock (store.SyncRoot)
        {
            if (!store.Categories.Any(c => c.Id == categoryId))
            {
                throw new KeyNotFoundException($"Category with Id {categoryId} not found");
            }

            var ordersCount = 0;
            var totalAmount = 0m;

            foreach (var order in store.Orders)
            {
                if (order.CreatedAt < periodStart || order.CreatedAt > periodEnd)
                {
                    continue;
                }

                var hasCategoryDish = false;
                foreach (var item in order.Items)
                {
                    if (item.Dish.Category.Id == categoryId)
                    {
                        hasCategoryDish = true;
                        totalAmount += item.SubTotal;
                    }
                }

                if (hasCategoryDish)
                {
                    ordersCount++;
                }
            }

            var averageAmount = ordersCount > 0 ? Math.Round(totalAmount / ordersCount, 2) : 0m;

            return mapper.Map<CategorySummaryDto>((categoryId, ordersCount, totalAmount, averageAmount));
        }
    }

    /// <inheritdoc/>
    public TopCustomerDto GetTopSpendingCustomer()
    {
        lock (store.SyncRoot)
        {
            if (store.Orders.Count == 0)
            {
                throw new KeyNotFoundException("No orders found to determine top spending customer");
            }

            var top = store.Orders
                .GroupBy(o => o.Customer)
                .Select(g => (Customer: g.Key, TotalSpent: g.Sum(o => o.TotalAmount)))
                .MaxBy(x => x.TotalSpent);

            return mapper.Map<TopCustomerDto>(top);
        }
    }
}
