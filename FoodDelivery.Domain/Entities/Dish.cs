namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Блюдо из ресторана.
/// </summary>
public sealed class Dish
{
    /// <summary>
    /// Уникальный идентификатор блюда.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Название блюда.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Вес блюда в граммах.
    /// </summary>
    public required int WeightGrams { get; set; }

    /// <summary>
    /// Цена блюда в рублях.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// Идентификатор категории блюда.
    /// </summary>
    public required int CategoryId { get; set; }

    /// <summary>
    /// Навигационное свойство категории блюда.
    /// </summary>
    public required Category Category { get; set; }

    /// <summary>
    /// Идентификатор ресторана, к которому относится блюдо.
    /// </summary>
    public required int RestaurantId { get; set; }

    /// <summary>
    /// Навигационное свойство ресторана.
    /// </summary>
    public required Restaurant Restaurant { get; set; }
}