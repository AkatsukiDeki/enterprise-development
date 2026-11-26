using FoodDelivery.Api;
using FoodDelivery.Application;
using FoodDelivery.Application.Contracts.Analytics;
using FoodDelivery.Application.Contracts.Categories;
using FoodDelivery.Application.Contracts.Customers;
using FoodDelivery.Application.Contracts.Dishes;
using FoodDelivery.Application.Contracts.Orders;
using FoodDelivery.Application.Contracts.Restaurants;
using FoodDelivery.Application.Services;
using FoodDelivery.Domain.Data;
using FoodDelivery.Infrastructure.InMemory;
using Mapster;
using MapsterMapper;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var mapsterConfig = new TypeAdapterConfig();
new MapsterConfig().Register(mapsterConfig);

builder.Services.AddSingleton(mapsterConfig);
builder.Services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<TypeAdapterConfig>()));

builder.Services.AddSingleton<TestData>();
builder.Services.AddSingleton<IFoodDeliveryStore, InMemoryFoodDeliveryStore>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
