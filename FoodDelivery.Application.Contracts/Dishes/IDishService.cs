namespace FoodDelivery.Application.Contracts.Dishes;

/// <summary>
/// Контракт сервиса для работы с блюдами.
/// </summary>
public interface IDishService : IApplicationService<DishDto, DishCreateUpdateDto, int>;
