using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Application.Contracts.Dishes;

/// <summary>
/// DTO для создания или обновления блюда.
/// </summary>
/// <param name="Name">Название блюда.</param>
/// <param name="Description">Описание блюда.</param>
/// <param name="WeightGrams">Вес блюда в граммах.</param>
/// <param name="Price">Цена блюда.</param>
/// <param name="CategoryId">Идентификатор категории.</param>
/// <param name="RestaurantId">Идентификатор ресторана.</param>
public sealed record DishCreateUpdateDto(
    [Required] string Name,
    string? Description,
    [Range(1, int.MaxValue)] int WeightGrams,
    [Range(0.01, double.MaxValue)] decimal Price,
    [Range(1, int.MaxValue)] int CategoryId,
    [Range(1, int.MaxValue)] int RestaurantId);
