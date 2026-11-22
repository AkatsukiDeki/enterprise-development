using FoodDelivery.Application.Contracts.Categories;
using FoodDelivery.Application.Contracts.Restaurants;

namespace FoodDelivery.Application.Contracts.Dishes;

/// <summary>
/// DTO блюда из меню ресторана.
/// </summary>
/// <param name="Id">Уникальный идентификатор блюда.</param>
/// <param name="Name">Название блюда.</param>
/// <param name="Description">Описание блюда.</param>
/// <param name="WeightGrams">Вес блюда в граммах.</param>
/// <param name="Price">Цена блюда.</param>
/// <param name="Category">Категория блюда.</param>
/// <param name="Restaurant">Ресторан, к которому относится блюдо.</param>
public sealed record DishDto(
    int Id,
    string Name,
    string? Description,
    int WeightGrams,
    decimal Price,
    CategoryDto Category,
    RestaurantDto Restaurant) : IHasId<int>;
