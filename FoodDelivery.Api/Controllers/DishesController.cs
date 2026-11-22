using FoodDelivery.Application.Contracts.Dishes;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер управления блюдами.
/// </summary>
/// <param name="service">Сервис блюд.</param>
public sealed class DishesController(IDishService service)
    : CrudControllerBase<DishDto, DishCreateUpdateDto, IDishService>(service);
