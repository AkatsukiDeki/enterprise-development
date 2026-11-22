namespace FoodDelivery.Application.Contracts;

/// <summary>
/// Маркерный интерфейс для DTO с идентификатором.
/// </summary>
/// <typeparam name="TKey">Тип уникального идентификатора.</typeparam>
public interface IHasId<TKey>
    where TKey : struct
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public TKey Id { get; }
}
