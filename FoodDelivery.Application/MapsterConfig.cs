using FoodDelivery.Application.Contracts.Analytics;
using FoodDelivery.Application.Contracts.Categories;
using FoodDelivery.Application.Contracts.Customers;
using FoodDelivery.Application.Contracts.Dishes;
using FoodDelivery.Application.Contracts.Orders;
using FoodDelivery.Application.Contracts.Restaurants;
using FoodDelivery.Domain.Entities;
using Mapster;

namespace FoodDelivery.Application;

/// <summary>
/// Конфигурация маппинга доменных сущностей и DTO через Mapster.
/// </summary>
public class MapsterConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDto>()
            .TwoWays();

        config.NewConfig<CategoryCreateUpdateDto, Category>()
            .Ignore(dest => dest.Id)
            .Map(dest => dest.Name, src => src.Name.Trim());

        config.NewConfig<Restaurant, RestaurantDto>()
            .TwoWays();

        config.NewConfig<RestaurantCreateUpdateDto, Restaurant>()
            .Ignore(dest => dest.Id)
            .Map(dest => dest.Name, src => src.Name.Trim())
            .Map(dest => dest.Address, src => src.Address.Trim());

        config.NewConfig<Customer, CustomerDto>()
            .TwoWays();

        config.NewConfig<CustomerCreateUpdateDto, Customer>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.FullName)
            .Map(dest => dest.LastName, src => src.LastName.Trim())
            .Map(dest => dest.FirstName, src => src.FirstName.Trim())
            .Map(dest => dest.Patronymic, src => string.IsNullOrWhiteSpace(src.Patronymic) ? null : src.Patronymic.Trim())
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber.Trim())
            .Map(dest => dest.DeliveryAddress, src => src.DeliveryAddress.Trim());

        config.NewConfig<Dish, DishDto>()
            .TwoWays();

        config.NewConfig<DishCreateUpdateDto, Dish>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.Restaurant)
            .Map(dest => dest.Name, src => src.Name.Trim())
            .Map(dest => dest.Description, src => string.IsNullOrWhiteSpace(src.Description) ? null : src.Description.Trim());

        config.NewConfig<OrderItem, OrderItemDto>()
            .TwoWays();

        config.NewConfig<OrderItemCreateUpdateDto, OrderItem>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Dish)
            .Ignore(dest => dest.SubTotal);

        config.NewConfig<Order, OrderDto>()
            .TwoWays();

        config.NewConfig<OrderCreateUpdateDto, Order>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Customer)
            .Ignore(dest => dest.Restaurant)
            .Ignore(dest => dest.Items)
            .Ignore(dest => dest.TotalAmount)
            .Ignore(dest => dest.DeliveryDuration)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt ?? DateTime.UtcNow);

        config.NewConfig<(int CategoryId, int OrdersCount, decimal TotalAmount, decimal AverageAmount), CategorySummaryDto>()
            .MapWith(src => new CategorySummaryDto(src.CategoryId, src.OrdersCount, src.TotalAmount, src.AverageAmount));

        config.NewConfig<(Customer Customer, decimal TotalSpent), TopCustomerDto>()
            .MapWith(src => new TopCustomerDto(src.Customer.Id, src.Customer.FullName, src.TotalSpent));
    }
}
