using FoodDelivery.Application.Contracts.Restaurants;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Infrastructure.InMemory;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления ресторанами.
/// </summary>
/// <param name="store">Хранилище данных в памяти.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class RestaurantService(InMemoryFoodDeliveryStore store, IMapper mapper) : IRestaurantService
{
    /// <inheritdoc/>
    public Task<IReadOnlyList<RestaurantDto>> GetAll()
    {
        lock (store.SyncRoot)
        {
            IReadOnlyList<RestaurantDto> result = store.Restaurants
                .Select(mapper.Map<RestaurantDto>)
                .ToList();
            return Task.FromResult(result);
        }
    }

    /// <inheritdoc/>
    public Task<RestaurantDto> Get(int id)
    {
        lock (store.SyncRoot)
        {
            var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == id)
                ?? throw new KeyNotFoundException($"Restaurant with Id {id} not found");

            return Task.FromResult(mapper.Map<RestaurantDto>(restaurant));
        }
    }

    /// <inheritdoc/>
    public Task<RestaurantDto> Create(RestaurantCreateUpdateDto dto)
    {
        ValidateWorkingHours(dto);

        lock (store.SyncRoot)
        {
            var restaurant = mapper.Map<Restaurant>(dto);
            restaurant.Id = store.NextRestaurantId();

            store.Restaurants.Add(restaurant);
            return Task.FromResult(mapper.Map<RestaurantDto>(restaurant));
        }
    }

    /// <inheritdoc/>
    public Task<RestaurantDto> Update(int id, RestaurantCreateUpdateDto dto)
    {
        ValidateWorkingHours(dto);

        lock (store.SyncRoot)
        {
            var index = store.Restaurants.FindIndex(r => r.Id == id);
            if (index < 0)
            {
                throw new KeyNotFoundException($"Restaurant with Id {id} not found");
            }

            var restaurant = mapper.Map<Restaurant>(dto);
            restaurant.Id = id;
            store.Restaurants[index] = restaurant;

            return Task.FromResult(mapper.Map<RestaurantDto>(restaurant));
        }
    }

    /// <inheritdoc/>
    public Task Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == id)
                ?? throw new KeyNotFoundException($"Restaurant with Id {id} not found");

            if (store.Dishes.Any(d => d.Restaurant.Id == id) || store.Orders.Any(o => o.Restaurant.Id == id))
            {
                throw new InvalidOperationException($"Restaurant with Id {id} is referenced by existing dishes or orders and cannot be deleted.");
            }

            store.Restaurants.Remove(restaurant);
            return Task.CompletedTask;
        }
    }

    private static void ValidateWorkingHours(RestaurantCreateUpdateDto dto)
    {
        if (dto.OpeningTime >= dto.ClosingTime)
        {
            throw new ArgumentException("Время открытия ресторана должно быть раньше времени закрытия.");
        }
    }
}
