using FoodDelivery.Application.Contracts.Restaurants;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер управления ресторанами.
/// </summary>
/// <param name="service">Сервис ресторанов.</param>
public sealed class RestaurantsController(IRestaurantService service)
    : CrudControllerBase<RestaurantDto, RestaurantCreateUpdateDto, IRestaurantService>(service);
