namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Заказ в службе доставки.
/// </summary>
public sealed class Order
{
    private List<OrderItem> _items = [];

    /// <summary>
    /// Уникальный идентификатор заказа.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Клиент, оформивший заказ.
    /// </summary>
    public required Customer Customer { get; set; }

    /// <summary>
    /// Ресторан, в котором сделан заказ.
    /// </summary>
    public required Restaurant Restaurant { get; set; }

    /// <summary>
    /// Время оформления заказа.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Время фактической доставки заказа. Может быть null, если заказ еще выполняется.
    /// </summary>
    public DateTime? DeliveredAt { get; set; }

    /// <summary>
    /// Состав позиций заказа. Все блюда должны принадлежать ресторану заказа.
    /// </summary>
    public required List<OrderItem> Items
    {
        get => _items;
        set
        {
            if (value.Any(item => item.Dish.Restaurant.Id != Restaurant.Id))
            {
                throw new InvalidOperationException("Заказ не может содержать блюда из другого ресторана.");
            }
            _items = value;
        }
    }

    /// <summary>
    /// Вычисляемая итоговая сумма заказа.
    /// </summary>
    public decimal TotalAmount => Items.Sum(i => i.SubTotal);

    /// <summary>
    /// Длительность доставки. Возвращает null, если заказ еще не доставлен.
    /// </summary>
    public TimeSpan? DeliveryDuration => DeliveredAt.HasValue ? DeliveredAt.Value - CreatedAt : null;
}