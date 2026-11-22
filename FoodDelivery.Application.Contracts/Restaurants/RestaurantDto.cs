namespace FoodDelivery.Application.Contracts.Restaurants;

/// <summary>
/// DTO ресторана службы доставки.
/// </summary>
/// <param name="Id">Уникальный идентификатор ресторана.</param>
/// <param name="Name">Название ресторана.</param>
/// <param name="Address">Адрес ресторана.</param>
/// <param name="Rating">Рейтинг ресторана.</param>
/// <param name="OpeningTime">Время открытия.</param>
/// <param name="ClosingTime">Время закрытия.</param>
public sealed record RestaurantDto(
    int Id,
    string Name,
    string Address,
    double Rating,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime) : IHasId<int>;
