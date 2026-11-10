using System.Net;
using System.Net.Http.Json;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Server.Controllers;
using Xunit;

namespace FoodDelivery.Tests;

public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCategories_ReturnsOkAndList()
    {
        var ct = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync("/api/categories", ct);
        response.EnsureSuccessStatusCode();

        var categories = await response.Content.ReadFromJsonAsync<List<Category>>(cancellationToken: ct);
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreated()
    {
        var ct = TestContext.Current.CancellationToken;
        var newCategory = new Category { Id = 0, Name = "Восточная кухня" };
        var response = await _client.PostAsJsonAsync("/api/categories", newCategory, ct);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Category>(cancellationToken: ct);
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("Восточная кухня", created.Name);
    }

    [Fact]
    public async Task GetTop5Restaurants_ReturnsTopList()
    {
        var ct = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync("/api/analytics/top-restaurants", ct);
        response.EnsureSuccessStatusCode();

        var topRestaurantIds = await response.Content.ReadFromJsonAsync<List<int>>(cancellationToken: ct);
        Assert.NotNull(topRestaurantIds);
        Assert.Equal(5, topRestaurantIds.Count);
        Assert.Equal(2, topRestaurantIds[0]);
    }

    [Fact]
    public async Task GetOrdersWithMinDeliveryDuration_ReturnsFastestOrders()
    {
        var ct = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync("/api/analytics/fastest-orders", ct);
        response.EnsureSuccessStatusCode();

        var orderIds = await response.Content.ReadFromJsonAsync<List<int>>(cancellationToken: ct);
        Assert.NotNull(orderIds);
        Assert.NotEmpty(orderIds);
        Assert.Contains(11, orderIds);
    }

    [Fact]
    public async Task GetTopSpendingCustomer_ReturnsExpectedCustomer()
    {
        var ct = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync("/api/analytics/top-spending-customer", ct);
        response.EnsureSuccessStatusCode();

        var customer = await response.Content.ReadFromJsonAsync<TopCustomerDto>(cancellationToken: ct);
        Assert.NotNull(customer);
        Assert.Equal(1, customer.CustomerId);
    }

    [Fact]
    public async Task GetCategorySummary_WithDateBounds_FiltersCorrectly()
    {
        var ct = TestContext.Current.CancellationToken;
        var url = "/api/analytics/category-summary?categoryId=1&periodStart=2026-09-01T00:00:00&periodEnd=2026-09-30T23:59:59";
        var response = await _client.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var summary = await response.Content.ReadFromJsonAsync<CategorySummaryDto>(cancellationToken: ct);
        Assert.NotNull(summary);
        Assert.Equal(1, summary.CategoryId);
        Assert.True(summary.OrdersCount > 0);
        Assert.True(summary.TotalAmount > 0);
    }
}
