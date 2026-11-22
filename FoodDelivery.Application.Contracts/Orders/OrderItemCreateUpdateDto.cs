using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Application.Contracts.Orders;

/// <summary>
/// DTO для создания или обновления позиции заказа.
/// </summary>
/// <param name="DishId">Идентификатор блюда.</param>
/// <param name="Quantity">Количество единиц блюда.</param>
public sealed record OrderItemCreateUpdateDto(
    [Range(1, int.MaxValue)] int DishId,
    [Range(1, int.MaxValue)] int Quantity);
