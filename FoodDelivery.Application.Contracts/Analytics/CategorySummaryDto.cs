namespace FoodDelivery.Application.Contracts.Analytics;

/// <summary>
/// Сводная статистика заказов по категории блюд за период.
/// </summary>
/// <param name="CategoryId">Идентификатор категории.</param>
/// <param name="OrdersCount">Количество заказов.</param>
/// <param name="TotalAmount">Общая сумма продаж.</param>
/// <param name="AverageAmount">Средняя сумма по заказам.</param>
public sealed record CategorySummaryDto(
    int CategoryId,
    int OrdersCount,
    decimal TotalAmount,
    decimal AverageAmount);
