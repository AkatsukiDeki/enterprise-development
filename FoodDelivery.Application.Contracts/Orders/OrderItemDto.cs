using FoodDelivery.Application.Contracts.Dishes;

namespace FoodDelivery.Application.Contracts.Orders;

/// <summary>
/// DTO позиции в заказе.
/// </summary>
/// <param name="Id">Уникальный идентификатор позиции.</param>
/// <param name="Dish">Заказанное блюдо.</param>
/// <param name="Quantity">Количество единиц блюда.</param>
/// <param name="SubTotal">Вычисляемая стоимость позиции.</param>
public sealed record OrderItemDto(
    int Id,
    DishDto Dish,
    int Quantity,
    decimal SubTotal) : IHasId<int>;
