using FoodDelivery.Application.Contracts.Orders;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Infrastructure.InMemory;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления заказами.
/// </summary>
/// <param name="store">Хранилище данных в памяти.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class OrderService(InMemoryFoodDeliveryStore store, IMapper mapper) : IOrderService
{
    /// <inheritdoc/>
    public Task<IReadOnlyList<OrderDto>> GetAll()
    {
        lock (store.SyncRoot)
        {
            IReadOnlyList<OrderDto> result = store.Orders
                .Select(mapper.Map<OrderDto>)
                .ToList();
            return Task.FromResult(result);
        }
    }

    /// <inheritdoc/>
    public Task<OrderDto> Get(int id)
    {
        lock (store.SyncRoot)
        {
            var order = store.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Order with Id {id} not found");

            return Task.FromResult(mapper.Map<OrderDto>(order));
        }
    }

    /// <inheritdoc/>
    public Task<OrderDto> Create(OrderCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var createdAt = dto.CreatedAt ?? DateTime.UtcNow;
            var (customer, restaurant, items) = ResolveAndValidate(dto, createdAt);

            var order = mapper.Map<Order>(dto);
            order.Id = store.NextOrderId();
            order.Customer = customer;
            order.Restaurant = restaurant;
            order.CreatedAt = createdAt;
            order.Items = items;

            store.Orders.Add(order);
            return Task.FromResult(mapper.Map<OrderDto>(order));
        }
    }

    /// <inheritdoc/>
    public Task<OrderDto> Update(int id, OrderCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var index = store.Orders.FindIndex(o => o.Id == id);
            if (index < 0)
            {
                throw new KeyNotFoundException($"Order with Id {id} not found");
            }

            var existingOrder = store.Orders[index];
            var createdAt = dto.CreatedAt ?? existingOrder.CreatedAt;
            var (customer, restaurant, items) = ResolveAndValidate(dto, createdAt);

            var order = mapper.Map<Order>(dto);
            order.Id = id;
            order.Customer = customer;
            order.Restaurant = restaurant;
            order.CreatedAt = createdAt;
            order.Items = items;
            store.Orders[index] = order;

            return Task.FromResult(mapper.Map<OrderDto>(order));
        }
    }

    /// <inheritdoc/>
    public Task Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var order = store.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Order with Id {id} not found");

            store.Orders.Remove(order);
            return Task.CompletedTask;
        }
    }

    private (Customer Customer, Restaurant Restaurant, List<OrderItem> Items) ResolveAndValidate(
        OrderCreateUpdateDto dto,
        DateTime createdAt)
    {
        if (dto.DeliveredAt.HasValue && dto.DeliveredAt.Value < createdAt)
        {
            throw new ArgumentException("Время доставки не может быть раньше времени создания заказа.");
        }

        var customer = store.Customers.FirstOrDefault(c => c.Id == dto.CustomerId)
            ?? throw new KeyNotFoundException($"Customer with Id {dto.CustomerId} not found");

        var restaurant = store.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId)
            ?? throw new KeyNotFoundException($"Restaurant with Id {dto.RestaurantId} not found");

        var items = new List<OrderItem>(dto.Items.Count);
        foreach (var itemDto in dto.Items)
        {
            var dish = store.Dishes.FirstOrDefault(d => d.Id == itemDto.DishId)
                ?? throw new KeyNotFoundException($"Dish with Id {itemDto.DishId} not found");

            if (dish.Restaurant.Id != restaurant.Id)
            {
                throw new ArgumentException($"Блюдо '{dish.Name}' не принадлежит ресторану '{restaurant.Name}'.");
            }

            var orderItem = mapper.Map<OrderItem>(itemDto);
            orderItem.Id = store.NextOrderItemId();
            orderItem.Dish = dish;
            items.Add(orderItem);
        }

        return (customer, restaurant, items);
    }
}
