using FoodDelivery.Application.Contracts.Orders;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер управления заказами.
/// </summary>
/// <param name="service">Сервис заказов.</param>
public sealed class OrdersController(IOrderService service)
    : CrudControllerBase<OrderDto, OrderCreateUpdateDto, IOrderService>(service);
