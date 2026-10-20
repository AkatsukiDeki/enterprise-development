using FoodDelivery.Domain.Data;
using Xunit;

namespace FoodDelivery.Tests;

/// <summary>
/// Юнит-тесты аналитических запросов службы доставки еды.
/// </summary>
public sealed class FoodDeliveryTests : IClassFixture<TestData>
{
    private readonly TestData _data;

    /// <summary>
    /// Конструктор с внедрением фикстуры тестовых данных.
    /// </summary>
    public FoodDeliveryTests(TestData data)
    {
        _data = data;
    }

    /// <summary>
    /// Проверка: Вывести топ 5 ресторанов по количеству заказов.
    /// </summary>
    [Fact]
    public void Top5RestaurantsByOrderCount()
    {
        // Arrange
        var orders = _data.Orders;
        // Заказы: Тануки (Id 2) - 4, Перчини (Id 4) - 3, МясоRoob (Id 7) - 3, Додо (Id 1) - 3, Шоколадница (Id 8) - 2
        int[] expectedTop5Ids = [2, 1, 4, 7, 8];

        // Act
        var actualTop5Ids = orders
            .CountBy(order => order.Restaurant.Id)
            .OrderByDescending(pair => pair.Value)
            .ThenBy(pair => pair.Key)
            .Take(5)
            .Select(pair => pair.Key)
            .ToArray();

        // Assert
        Assert.Equal(expectedTop5Ids, actualTop5Ids);
    }

    /// <summary>
    /// Проверка: Вывести список заказов с минимальным временем доставки.
    /// </summary>
    [Fact]
    public void OrdersWithMinDeliveryDuration()
    {
        // Arrange
        var deliveredOrders = _data.Orders.Where(o => o.DeliveryDuration.HasValue).ToList();
        var expectedMinDuration = TimeSpan.FromMinutes(15);
        var expectedOrderId = 11;

        // Act
        var minDuration = deliveredOrders.Min(o => o.DeliveryDuration!.Value);
        var fastestOrders = deliveredOrders
            .Where(o => o.DeliveryDuration == minDuration)
            .ToList();

        // Assert
        Assert.Single(fastestOrders);
        Assert.Equal(expectedOrderId, fastestOrders[0].Id);
        Assert.Equal(expectedMinDuration, minDuration);
    }

    /// <summary>
    /// Проверка: Вывести идентификаторы клиентов, заказывавших в выбранном ресторане, упорядоченные по ФИО.
    /// </summary>
    [Fact]
    public void CustomersByRestaurantOrderedByName()
    {
        // Arrange
        var orders = _data.Orders;
        var targetRestaurantId = 2; // Тануки
        int[] expectedCustomerIds = [1, 3, 4];

        // Act
        var actualCustomerIds = orders
            .Where(o => o.Restaurant.Id == targetRestaurantId)
            .Select(o => o.Customer)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .Select(c => c.Id)
            .ToArray();

        // Assert
        Assert.Equal(expectedCustomerIds, actualCustomerIds);
    }

    /// <summary>
    /// Проверка: Вывести сводную информацию о заказах по указанной категории блюд за выбранный период.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории блюд.</param>
    /// <param name="expectedOrdersCount">Ожидаемое количество заказов с блюдами категории.</param>
    /// <param name="expectedTotalAmount">Ожидаемая сумма продаж блюд данной категории.</param>
    /// <param name="expectedAverageAmount">Ожидаемый средний чек позиции данной категории в заказе.</param>
    [Theory]
    [InlineData(1, 3, 1790.0, 596.67)]   // Пицца: Заказ 11 (550), Заказ 12 (550), Заказ 19 (690)
    [InlineData(2, 5, 5640.0, 1128.0)]   // Суши: Заказы 4 (1240), 5 (620), 6 (1780), 7 (1080), 17 (920)
    [InlineData(9, 2, 830.0, 415.0)]     // Завтраки: Заказ 13 (520), Заказ 14 (310)
    [InlineData(10, 3, 5630.0, 1876.67)] // Гриль: Заказы 1 (3220), 2 (1450), 3 (960)
    public void OrdersSummaryByCategoryForSelectedPeriod(
        int categoryId,
        int expectedOrdersCount,
        decimal expectedTotalAmount,
        decimal expectedAverageAmount)
    {
        // Arrange
        var orders = _data.Orders;
        var periodStart = new DateTime(2026, 9, 1, 0, 0, 0);
        var periodEnd = new DateTime(2026, 9, 30, 23, 59, 59);

        // Act
        var categoryOrders = orders
            .Where(o => o.CreatedAt >= periodStart && o.CreatedAt <= periodEnd)
            .Where(o => o.Items.Any(item => item.Dish.Category.Id == categoryId))
            .ToList();

        var ordersCount = categoryOrders.Count;

        var totalAmount = categoryOrders
            .SelectMany(o => o.Items)
            .Where(item => item.Dish.Category.Id == categoryId)
            .Sum(item => item.SubTotal);

        var averageAmount = ordersCount > 0 ? Math.Round(totalAmount / ordersCount, 2) : 0m;

        // Assert
        Assert.Equal(expectedOrdersCount, ordersCount);
        Assert.Equal(expectedTotalAmount, totalAmount);
        Assert.Equal(expectedAverageAmount, averageAmount);
    }

    /// <summary>
    /// Проверка: Вывести информацию о клиенте, который потратил наибольшую сумму за все время.
    /// </summary>
    [Fact]
    public void TopSpendingCustomer()
    {
        // Arrange
        var orders = _data.Orders;
        var expectedCustomerId = 1;

        // Act
        var topCustomerId = orders
            .GroupBy(o => o.Customer.Id)
            .Select(g => new
            {
                CustomerId = g.Key,
                TotalSpent = g.Sum(o => o.TotalAmount)
            })
            .OrderByDescending(x => x.TotalSpent)
            .First()
            .CustomerId;

        // Assert
        Assert.Equal(expectedCustomerId, topCustomerId);
    }

    /// <summary>
    /// Проверка: Заказ не может содержать блюдо из чужого ресторана.
    /// </summary>
    [Fact]
    public void OrderCannotContainDishesFromDifferentRestaurant()
    {
        // Arrange
        var dodoPizza = _data.Dishes[0];
        var tanukiRestaurant = _data.Restaurants[1];

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = new Domain.Entities.Order
            {
                Id = 999,
                Customer = _data.Customers[0],
                Restaurant = tanukiRestaurant,
                CreatedAt = DateTime.UtcNow,
                DeliveredAt = DateTime.UtcNow.AddMinutes(30),
                Items =
                [
                    new() { Id = 999, Dish = dodoPizza, Quantity = 1 }
                ]
            };
        });
    }
}