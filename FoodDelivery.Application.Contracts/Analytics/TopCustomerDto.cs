namespace FoodDelivery.Application.Contracts.Analytics;

/// <summary>
/// Информация о клиенте с наибольшей суммой заказов.
/// </summary>
/// <param name="CustomerId">Идентификатор клиента.</param>
/// <param name="FullName">Полное имя клиента.</param>
/// <param name="TotalSpent">Общая потраченная сумма.</param>
public sealed record TopCustomerDto(
    int CustomerId,
    string FullName,
    decimal TotalSpent);
