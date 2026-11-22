namespace FoodDelivery.Application.Contracts.Orders;

/// <summary>
/// Контракт сервиса для работы с заказами.
/// </summary>
public interface IOrderService : IApplicationService<OrderDto, OrderCreateUpdateDto, int>;
