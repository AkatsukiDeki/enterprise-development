using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Application.Contracts.Categories;

/// <summary>
/// DTO для создания или обновления категории блюд.
/// </summary>
/// <param name="Name">Название категории.</param>
public sealed record CategoryCreateUpdateDto(
    [Required] [MinLength(1)] string Name);
