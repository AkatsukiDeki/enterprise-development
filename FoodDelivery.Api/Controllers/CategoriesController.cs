using FoodDelivery.Application.Contracts.Categories;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер управления категориями блюд.
/// </summary>
/// <param name="service">Сервис категорий.</param>
public sealed class CategoriesController(ICategoryService service)
    : CrudControllerBase<CategoryDto, CategoryCreateUpdateDto, ICategoryService>(service);
