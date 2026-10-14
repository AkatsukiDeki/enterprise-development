namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Позиция в заказе.
/// </summary>
public sealed class OrderItem
{
    /// <summary>
    /// Уникальный идентификатор позиции.
    /// </summary>
    public required int Id { get; set; }
    
    /// <summary>
    /// Заказанное блюдо.
    /// </summary>
    public required Dish Dish { get; set; }

    /// <summary>
    /// Количество единиц блюда.
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Вычисляемая стоимость позиции.
    /// </summary>
    public decimal SubTotal => Quantity * Dish.Price;
}