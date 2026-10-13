namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Категория блюда (справочник).
/// </summary>
public sealed class Category
{
    /// <summary>
    /// Уникальный идентификатор категории.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Название категории.
    /// </summary>
    public required string Name { get; set; }
}