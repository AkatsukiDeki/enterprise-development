using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Application.Contracts.Orders;

/// <summary>
/// DTO для создания или обновления заказа.
/// </summary>
/// <param name="CustomerId">Идентификатор клиента.</param>
/// <param name="RestaurantId">Идентификатор ресторана.</param>
/// <param name="CreatedAt">Время создания заказа.</param>
/// <param name="DeliveredAt">Время доставки заказа.</param>
/// <param name="Items">Позиции заказа.</param>
public sealed record OrderCreateUpdateDto(
    [Range(1, int.MaxValue)] int CustomerId,
    [Range(1, int.MaxValue)] int RestaurantId,
    DateTime? CreatedAt,
    DateTime? DeliveredAt,
    [Required] [MinLength(1)] IReadOnlyList<OrderItemCreateUpdateDto> Items);
