namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Ресторан службы доставки.
/// </summary>
public sealed class Restaurant
{
    /// <summary>
    /// Уникальный идентификатор ресторана.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Название заведения.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Фактический адрес ресторана.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Рейтинг заведения.
    /// </summary>
    public required double Rating { get; set; }

    /// <summary>
    /// Время открытия.
    /// </summary>
    public required TimeOnly OpeningTime { get; set; }

    /// <summary>
    /// Время закрытия.
    /// </summary>
    public required TimeOnly ClosingTime { get; set; }
}