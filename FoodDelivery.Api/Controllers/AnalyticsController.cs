using FoodDelivery.Application.Contracts.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер аналитических запросов службы доставки.
/// </summary>
/// <param name="analyticsService">Сервис аналитики.</param>
[ApiController]
[Route("api/[controller]")]
public sealed class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    /// <summary>
    /// Топ-5 ресторанов по количеству заказов.
    /// </summary>
    [HttpGet("top-restaurants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<int>> GetTop5Restaurants()
    {
        var result = analyticsService.GetTop5Restaurants();
        return Ok(result);
    }

    /// <summary>
    /// Идентификаторы заказов с минимальным временем доставки.
    /// </summary>
    [HttpGet("fastest-orders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<int>> GetFastestOrders()
    {
        var result = analyticsService.GetFastestOrders();
        return Ok(result);
    }

    /// <summary>
    /// Идентификаторы клиентов, заказывавших в указанном ресторане, отсортированные по ФИО.
    /// </summary>
    /// <param name="restaurantId">Идентификатор ресторана.</param>
    [HttpGet("restaurant-customers/{restaurantId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IReadOnlyList<int>> GetCustomersByRestaurant(int restaurantId)
    {
        var result = analyticsService.GetCustomersByRestaurant(restaurantId);
        return Ok(result);
    }

    /// <summary>
    /// Сводная информация по категории блюд за указанный период времени.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории блюд.</param>
    /// <param name="periodStart">Начало периода.</param>
    /// <param name="periodEnd">Окончание периода.</param>
    [HttpGet("category-summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CategorySummaryDto> GetCategorySummary(
        [FromQuery] int categoryId,
        [FromQuery] DateTime periodStart,
        [FromQuery] DateTime periodEnd)
    {
        var result = analyticsService.GetCategorySummary(categoryId, periodStart, periodEnd);
        return Ok(result);
    }

    /// <summary>
    /// Клиент, потративший наибольшую сумму за всё время.
    /// </summary>
    [HttpGet("top-spending-customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<TopCustomerDto> GetTopSpendingCustomer()
    {
        var result = analyticsService.GetTopSpendingCustomer();
        return Ok(result);
    }
}
