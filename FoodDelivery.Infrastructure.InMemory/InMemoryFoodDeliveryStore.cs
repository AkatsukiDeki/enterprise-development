using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Infrastructure.InMemory;

/// <summary>
/// Потокобезопасное хранилище данных в памяти поверх <see cref="TestData"/>.
/// </summary>
public sealed class InMemoryFoodDeliveryStore
{
    private int _nextCategoryId;
    private int _nextRestaurantId;
    private int _nextCustomerId;
    private int _nextDishId;
    private int _nextOrderId;
    private int _nextOrderItemId;

    /// <summary>
    /// Инициализирует хранилище начальными тестовыми данными.
    /// </summary>
    /// <param name="seedData">Набор начальных тестовых данных.</param>
    public InMemoryFoodDeliveryStore(TestData seedData)
    {
        Categories = [.. seedData.Categories];
        Restaurants = [.. seedData.Restaurants];
        Customers = [.. seedData.Customers];
        Dishes = [.. seedData.Dishes];
        Orders = [.. seedData.Orders];

        _nextCategoryId = Categories.Count > 0 ? Categories.Max(c => c.Id) : 0;
        _nextRestaurantId = Restaurants.Count > 0 ? Restaurants.Max(r => r.Id) : 0;
        _nextCustomerId = Customers.Count > 0 ? Customers.Max(c => c.Id) : 0;
        _nextDishId = Dishes.Count > 0 ? Dishes.Max(d => d.Id) : 0;
        _nextOrderId = Orders.Count > 0 ? Orders.Max(o => o.Id) : 0;
        _nextOrderItemId = Orders.SelectMany(o => o.Items).Select(i => i.Id).DefaultIfEmpty(0).Max();
    }

    /// <summary>
    /// Объект синхронизации для потокобезопасного доступа к коллекциям.
    /// </summary>
    public Lock SyncRoot { get; } = new();

    /// <summary>
    /// Коллекция категорий блюд.
    /// </summary>
    public List<Category> Categories { get; }

    /// <summary>
    /// Коллекция ресторанов.
    /// </summary>
    public List<Restaurant> Restaurants { get; }

    /// <summary>
    /// Коллекция клиентов.
    /// </summary>
    public List<Customer> Customers { get; }

    /// <summary>
    /// Коллекция блюд.
    /// </summary>
    public List<Dish> Dishes { get; }

    /// <summary>
    /// Коллекция заказов.
    /// </summary>
    public List<Order> Orders { get; }

    /// <summary>
    /// Генерирует следующий идентификатор категории.
    /// </summary>
    /// <returns>Новый уникальный идентификатор категории.</returns>
    public int NextCategoryId() => ++_nextCategoryId;

    /// <summary>
    /// Генерирует следующий идентификатор ресторана.
    /// </summary>
    /// <returns>Новый уникальный идентификатор ресторана.</returns>
    public int NextRestaurantId() => ++_nextRestaurantId;

    /// <summary>
    /// Генерирует следующий идентификатор клиента.
    /// </summary>
    /// <returns>Новый уникальный идентификатор клиента.</returns>
    public int NextCustomerId() => ++_nextCustomerId;

    /// <summary>
    /// Генерирует следующий идентификатор блюда.
    /// </summary>
    /// <returns>Новый уникальный идентификатор блюда.</returns>
    public int NextDishId() => ++_nextDishId;

    /// <summary>
    /// Генерирует следующий идентификатор заказа.
    /// </summary>
    /// <returns>Новый уникальный идентификатор заказа.</returns>
    public int NextOrderId() => ++_nextOrderId;

    /// <summary>
    /// Генерирует следующий идентификатор позиции заказа.
    /// </summary>
    /// <returns>Новый уникальный идентификатор позиции заказа.</returns>
    public int NextOrderItemId() => ++_nextOrderItemId;
}
