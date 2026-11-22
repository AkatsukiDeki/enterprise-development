namespace FoodDelivery.Application.Contracts.Categories;

/// <summary>
/// DTO категории блюд.
/// </summary>
/// <param name="Id">Уникальный идентификатор категории.</param>
/// <param name="Name">Название категории.</param>
public sealed record CategoryDto(int Id, string Name) : IHasId<int>;
