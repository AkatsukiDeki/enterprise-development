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
        var orders = _data.Orders;

        var actualTop5Ids = orders
            .CountBy(order => order.Restaurant.Id)
            .OrderByDescending(pair => pair.Value)
            .ThenBy(pair => pair.Key)
            .Take(5)
            .Select(pair => pair.Key)
            .ToArray();

        int[] expected = [2, 4, 7, 1, 8];

        Assert.Equal(expected, actualTop5Ids);
    }

    /// <summary>
    /// Проверка: Вывести список заказов с минимальным временем доставки.
    /// </summary>
    [Fact]
    public void OrdersWithMinDeliveryDuration()
    {
        var orders = _data.Orders;
        var minDuration = orders.Min(o => o.DeliveryDuration);

        var fastestOrders = orders
            .Where(o => o.DeliveryDuration == minDuration)
            .ToList();

        Assert.Single(fastestOrders);
        Assert.Equal(11, fastestOrders[0].Id);
        Assert.Equal(TimeSpan.FromMinutes(15), minDuration);
    }

    /// <summary>
    /// Проверка: Вывести сведения обо всех клиентах, заказывавших в выбранном ресторане, упорядочить по ФИО.
    /// </summary>
    [Fact]
    public void CustomersByRestaurantOrderedByName()
    {
        var orders = _data.Orders;
        var targetRestaurantId = 2;

        var customers = orders
            .Where(o => o.Restaurant.Id == targetRestaurantId)
            .Select(o => o.Customer)
            .DistinctBy(c => c.Id)
            .OrderBy(c => c.FullName)
            .ToList();

        var actualNames = customers.Select(c => c.FullName).ToArray();
        string[] expectedNames =
        [
            "Иванов Иван Иванович",
            "Сидоров Алексей Михайлович",
            "Смирнов Дмитрий Александрович"
        ];

        Assert.Equal(expectedNames, actualNames);
    }

    /// <summary>
    /// Проверка: Вывести сводную информацию о заказах по указанной категории блюд за выбранный период.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории блюд.</param>
    /// <param name="expectedOrdersCount">Ожидаемое количество заказов.</param>
    /// <param name="expectedTotalAmount">Ожидаемая суммарная стоимость заказов.</param>
    [Theory]
    [InlineData(1, 2, 1650.0)]
    [InlineData(2, 4, 4340.0)]
    [InlineData(10, 3, 5800.0)]
    public void OrdersSummaryByCategoryForSelectedPeriod(int categoryId, int expectedOrdersCount, decimal expectedTotalAmount)
    {
        var orders = _data.Orders;
        var periodStart = new DateTime(2026, 9, 1);
        var periodEnd = new DateTime(2026, 9, 30);

        var categoryOrders = orders
            .Where(o => o.CreatedAt >= periodStart && o.CreatedAt <= periodEnd)
            .Where(o => o.Items.Any(item => item.Dish.Category.Id == categoryId))
            .ToList();

        var ordersCount = categoryOrders.Count;
        var totalAmount = categoryOrders.Sum(o => o.TotalAmount);
        var averageAmount = ordersCount > 0 ? totalAmount / ordersCount : 0m;

        Assert.Equal(expectedOrdersCount, ordersCount);
        Assert.Equal(expectedTotalAmount, totalAmount);
        Assert.True(averageAmount > 0);
    }

    /// <summary>
    /// Проверка: Вывести информацию о клиенте, который потратил наибольшую сумму за все время.
    /// </summary>
    [Fact]
    public void TopSpendingCustomer()
    {
        var orders = _data.Orders;

        var topCustomer = orders
            .GroupBy(o => o.Customer)
            .Select(g => new { Customer = g.Key, TotalSpent = g.Sum(o => o.TotalAmount) })
            .OrderByDescending(x => x.TotalSpent)
            .First()
            .Customer;

        Assert.Equal(1, topCustomer.Id);
        Assert.Equal("Иванов Иван Иванович", topCustomer.FullName);
    }

    /// <summary>
    /// Проверка: Заказ не может содержать блюдо из чужого ресторана.
    /// </summary>
    [Fact]
    public void OrderCannotContainDishesFromDifferentRestaurant()
    {
        var dodoPizza = _data.Dishes[0];
        var tanukiRestaurant = _data.Restaurants[1];

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