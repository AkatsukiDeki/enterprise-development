namespace FoodDelivery.Application.Contracts.Restaurants;

/// <summary>
/// Контракт сервиса для работы с ресторанами.
/// </summary>
public interface IRestaurantService : IApplicationService<RestaurantDto, RestaurantCreateUpdateDto, int>;
