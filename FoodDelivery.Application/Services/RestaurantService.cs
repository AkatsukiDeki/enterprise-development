using FoodDelivery.Application.Contracts.Restaurants;
using FoodDelivery.Domain.Entities;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления ресторанами.
/// </summary>
/// <param name="store">Хранилище данных.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class RestaurantService(IFoodDeliveryStore store, IMapper mapper) : IRestaurantService
{
    /// <inheritdoc/>
    public IReadOnlyList<RestaurantDto> GetAll()
    {
        lock (store.SyncRoot)
        {
            return store.Restaurants
                .Select(mapper.Map<RestaurantDto>)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public RestaurantDto Get(int id)
    {
        lock (store.SyncRoot)
        {
            var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == id)
                ?? throw new KeyNotFoundException($"Restaurant with Id {id} not found");

            return mapper.Map<RestaurantDto>(restaurant);
        }
    }

    /// <inheritdoc/>
    public RestaurantDto Create(RestaurantCreateUpdateDto dto)
    {
        ValidateWorkingHours(dto);

        lock (store.SyncRoot)
        {
            var restaurant = mapper.Map<Restaurant>(dto);
            restaurant.Id = store.NextRestaurantId();

            store.Restaurants.Add(restaurant);
            return mapper.Map<RestaurantDto>(restaurant);
        }
    }

    /// <inheritdoc/>
    public RestaurantDto Update(int id, RestaurantCreateUpdateDto dto)
    {
        ValidateWorkingHours(dto);

        lock (store.SyncRoot)
        {
            var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == id)
                ?? throw new KeyNotFoundException($"Restaurant with Id {id} not found");

            mapper.Map(dto, restaurant);
            return mapper.Map<RestaurantDto>(restaurant);
        }
    }

    /// <inheritdoc/>
    public void Delete(int id)
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
        }
    }

    private static void ValidateWorkingHours(RestaurantCreateUpdateDto dto)
    {
        if (dto.OpeningTime >= dto.ClosingTime)
        {
            throw new ArgumentException("Restaurant opening time must be earlier than closing time.");
        }
    }
}
