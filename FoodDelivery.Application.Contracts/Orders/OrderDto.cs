using FoodDelivery.Application.Contracts.Customers;
using FoodDelivery.Application.Contracts.Restaurants;

namespace FoodDelivery.Application.Contracts.Orders;

/// <summary>
/// DTO заказа службы доставки.
/// </summary>
/// <param name="Id">Уникальный идентификатор заказа.</param>
/// <param name="Customer">Клиент, оформивший заказ.</param>
/// <param name="Restaurant">Ресторан, в котором оформлен заказ.</param>
/// <param name="CreatedAt">Время оформления заказа.</param>
/// <param name="DeliveredAt">Время доставки заказа.</param>
/// <param name="DeliveryDuration">Вычисляемая длительность доставки.</param>
/// <param name="TotalAmount">Вычисляемая итоговая сумма заказа.</param>
/// <param name="Items">Позиции заказа.</param>
public sealed record OrderDto(
    int Id,
    CustomerDto Customer,
    RestaurantDto Restaurant,
    DateTime CreatedAt,
    DateTime? DeliveredAt,
    TimeSpan? DeliveryDuration,
    decimal TotalAmount,
    IReadOnlyList<OrderItemDto> Items) : IHasId<int>;
