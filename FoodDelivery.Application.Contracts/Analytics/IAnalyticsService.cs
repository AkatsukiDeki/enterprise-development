namespace FoodDelivery.Application.Contracts.Analytics;

/// <summary>
/// Контракт сервиса аналитических запросов службы доставки.
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Возвращает идентификаторы топ-5 ресторанов по количеству заказов.
    /// </summary>
    /// <returns>Список идентификаторов топ-5 ресторанов.</returns>
    public Task<IReadOnlyList<int>> GetTop5RestaurantsAsync();

    /// <summary>
    /// Возвращает идентификаторы заказов с минимальным временем доставки.
    /// </summary>
    /// <returns>Список идентификаторов самых быстрых заказов.</returns>
    public Task<IReadOnlyList<int>> GetFastestOrdersAsync();

    /// <summary>
    /// Возвращает идентификаторы клиентов, заказывавших в указанном ресторане, отсортированные по ФИО.
    /// </summary>
    /// <param name="restaurantId">Идентификатор ресторана.</param>
    /// <returns>Список идентификаторов клиентов, отсортированный по ФИО.</returns>
    public Task<IReadOnlyList<int>> GetCustomersByRestaurantAsync(int restaurantId);

    /// <summary>
    /// Возвращает сводную информацию по категории блюд за указанный период.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории блюд.</param>
    /// <param name="periodStart">Начало периода.</param>
    /// <param name="periodEnd">Окончание периода.</param>
    /// <returns>Сводная статистика заказов по категории за период.</returns>
    public Task<CategorySummaryDto> GetCategorySummaryAsync(
        int categoryId,
        DateTime periodStart,
        DateTime periodEnd);

    /// <summary>
    /// Возвращает клиента, потратившего наибольшую сумму за всё время.
    /// </summary>
    /// <returns>Информация о клиенте с наибольшей суммой заказов.</returns>
    public Task<TopCustomerDto> GetTopSpendingCustomerAsync();
}
