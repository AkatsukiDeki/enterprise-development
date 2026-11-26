using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application;

/// <summary>
/// Контракт потокобезопасного хранилища данных службы доставки.
/// </summary>
public interface IFoodDeliveryStore
{
    /// <summary>
    /// Объект синхронизации для потокобезопасного доступа к коллекциям.
    /// </summary>
    public Lock SyncRoot { get; }

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
    public int NextCategoryId();

    /// <summary>
    /// Генерирует следующий идентификатор ресторана.
    /// </summary>
    /// <returns>Новый уникальный идентификатор ресторана.</returns>
    public int NextRestaurantId();

    /// <summary>
    /// Генерирует следующий идентификатор клиента.
    /// </summary>
    /// <returns>Новый уникальный идентификатор клиента.</returns>
    public int NextCustomerId();

    /// <summary>
    /// Генерирует следующий идентификатор блюда.
    /// </summary>
    /// <returns>Новый уникальный идентификатор блюда.</returns>
    public int NextDishId();

    /// <summary>
    /// Генерирует следующий идентификатор заказа.
    /// </summary>
    /// <returns>Новый уникальный идентификатор заказа.</returns>
    public int NextOrderId();

    /// <summary>
    /// Генерирует следующий идентификатор позиции заказа.
    /// </summary>
    /// <returns>Новый уникальный идентификатор позиции заказа.</returns>
    public int NextOrderItemId();
}
