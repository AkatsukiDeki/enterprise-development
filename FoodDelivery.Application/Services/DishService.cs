using FoodDelivery.Application.Contracts.Dishes;
using FoodDelivery.Domain.Entities;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления блюдами.
/// </summary>
/// <param name="store">Хранилище данных.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class DishService(IFoodDeliveryStore store, IMapper mapper) : IDishService
{
    /// <inheritdoc/>
    public IReadOnlyList<DishDto> GetAll()
    {
        lock (store.SyncRoot)
        {
            return store.Dishes
                .Select(mapper.Map<DishDto>)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public DishDto Get(int id)
    {
        lock (store.SyncRoot)
        {
            var dish = store.Dishes.FirstOrDefault(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Dish with Id {id} not found");

            return mapper.Map<DishDto>(dish);
        }
    }

    /// <inheritdoc/>
    public DishDto Create(DishCreateUpdateDto dto)
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
            return mapper.Map<DishDto>(dish);
        }
    }

    /// <inheritdoc/>
    public DishDto Update(int id, DishCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var dish = store.Dishes.FirstOrDefault(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Dish with Id {id} not found");

            var category = store.Categories.FirstOrDefault(c => c.Id == dto.CategoryId)
                ?? throw new KeyNotFoundException($"Category with Id {dto.CategoryId} not found");

            var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId)
                ?? throw new KeyNotFoundException($"Restaurant with Id {dto.RestaurantId} not found");

            if (dish.Restaurant.Id != restaurant.Id &&
                store.Orders.Any(o => o.Items.Any(i => i.Dish.Id == id)))
            {
                throw new InvalidOperationException($"Dish with Id {id} is referenced by existing orders and cannot change restaurant.");
            }

            mapper.Map(dto, dish);
            dish.Category = category;
            dish.Restaurant = restaurant;

            return mapper.Map<DishDto>(dish);
        }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var dish = store.Dishes.FirstOrDefault(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Dish with Id {id} not found");

            if (store.Orders.Any(o => o.Items.Any(i => i.Dish.Id == id)))
            {
                throw new InvalidOperationException($"Dish with Id {id} is referenced by existing orders and cannot be deleted.");
            }

            store.Dishes.Remove(dish);
        }
    }
}
