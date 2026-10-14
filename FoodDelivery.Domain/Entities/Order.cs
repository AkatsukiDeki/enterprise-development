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
    /// Навигационное свойство клиента.
    /// </summary>
    public required Customer Customer { get; set; }

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
    /// Состав позиций заказа. Все блюда должны принадлежать ресторану заказа
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
    /// Длительность доставки.
    /// </summary>
    public TimeSpan DeliveryDuration => DeliveredAt - CreatedAt;
}