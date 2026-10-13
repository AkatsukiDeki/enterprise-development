namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Заказ в службе доставки.
/// </summary>
public sealed class Order
{
    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public required int CustomerId { get; set; }

    /// <summary>
    /// Навигационное свойство клиента.
    /// </summary>
    public required Customer Customer { get; set; }

    /// <summary>
    /// Идентификатор ресторана.
    /// </summary>
    public required int RestaurantId { get; set; }

    /// <summary>
    /// Навигационное свойство ресторана.
    /// </summary>
    public required Restaurant Restaurant { get; set; }

    /// <summary>
    /// Время оформления заказа.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Время фактической доставки заказа.
    /// </summary>
    public required DateTime DeliveredAt { get; set; }

    /// <summary>
    /// Состав позиций заказа.
    /// </summary>
    public required List<OrderItem> Items { get; set; }

    /// <summary>
    /// Вычисляемая итоговая сумма заказа.
    /// </summary>
    public decimal TotalAmount => Items.Sum(i => i.SubTotal);

    /// <summary>
    /// Длительность доставки.
    /// </summary>
    public TimeSpan DeliveryDuration => DeliveredAt - CreatedAt;
}