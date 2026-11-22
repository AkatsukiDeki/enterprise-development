using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Application.Contracts.Restaurants;

/// <summary>
/// DTO для создания или обновления ресторана.
/// </summary>
/// <param name="Name">Название ресторана.</param>
/// <param name="Address">Адрес ресторана.</param>
/// <param name="Rating">Рейтинг ресторана (от 0 до 5).</param>
/// <param name="OpeningTime">Время открытия.</param>
/// <param name="ClosingTime">Время закрытия.</param>
public sealed record RestaurantCreateUpdateDto(
    [Required] [MinLength(1)] string Name,
    [Required] [MinLength(1)] string Address,
    [Range(0.0, 5.0)] double Rating,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime);
