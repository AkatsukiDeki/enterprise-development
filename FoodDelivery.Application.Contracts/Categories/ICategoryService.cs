namespace FoodDelivery.Application.Contracts.Categories;

/// <summary>
/// Контракт сервиса для работы с категориями блюд.
/// </summary>
public interface ICategoryService : IApplicationService<CategoryDto, CategoryCreateUpdateDto, int>;
