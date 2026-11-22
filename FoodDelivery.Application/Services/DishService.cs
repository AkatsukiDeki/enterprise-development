using FoodDelivery.Application.Contracts.Dishes;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Infrastructure.InMemory;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления блюдами.
/// </summary>
/// <param name="store">Хранилище данных в памяти.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class DishService(InMemoryFoodDeliveryStore store, IMapper mapper) : IDishService
{
    /// <inheritdoc/>
    public Task<IReadOnlyList<DishDto>> GetAll()
    {
        lock (store.SyncRoot)
        {
            IReadOnlyList<DishDto> result = store.Dishes
                .Select(mapper.Map<DishDto>)
                .ToList();
            return Task.FromResult(result);
        }
    }

    /// <inheritdoc/>
    public Task<DishDto> Get(int id)
    {
        lock (store.SyncRoot)
        {
            var dish = store.Dishes.FirstOrDefault(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Dish with Id {id} not found");

            return Task.FromResult(mapper.Map<DishDto>(dish));
        }
    }

    /// <inheritdoc/>
    public Task<DishDto> Create(DishCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var category = store.Categories.FirstOrDefault(c => c.Id == dto.CategoryId)
                ?? throw new KeyNotFoundException($"Category with Id {dto.CategoryId} not found");

            var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with Id {dto.RestaurantId} not found");

            var dish = mapper.Map<Dish>(dto);
            dish.Id = store.NextDishId();
            dish.Category = category;
            dish.Restaurant = restaurant;

            store.Dishes.Add(dish);
            return Task.FromResult(mapper.Map<DishDto>(dish));
        }
    }

    /// <inheritdoc/>
    public Task<DishDto> Update(int id, DishCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var index = store.Dishes.FindIndex(d => d.Id == id);
            if (index < 0)
            {
                throw new KeyNotFoundException($"Dish with Id {id} not found");
            }

            var category = store.Categories.FirstOrDefault(c => c.Id == dto.CategoryId)
                ?? throw new KeyNotFoundException($"Category with Id {dto.CategoryId} not found");

            var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with Id {dto.RestaurantId} not found");

            var dish = mapper.Map<Dish>(dto);
            dish.Id = id;
            dish.Category = category;
            dish.Restaurant = restaurant;
            store.Dishes[index] = dish;

            return Task.FromResult(mapper.Map<DishDto>(dish));
        }
    }

    /// <inheritdoc/>
    public Task Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var dish = store.Dishes.FirstOrDefault(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Dish with Id {id} not found");

            store.Dishes.Remove(dish);
            return Task.CompletedTask;
        }
    }
}
