using FoodDelivery.Application.Contracts.Orders;
using FoodDelivery.Domain.Entities;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления заказами.
/// </summary>
/// <param name="store">Хранилище данных.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class OrderService(IFoodDeliveryStore store, IMapper mapper) : IOrderService
{
    /// <inheritdoc/>
    public IReadOnlyList<OrderDto> GetAll()
    {
        lock (store.SyncRoot)
        {
            return store.Orders
                .Select(mapper.Map<OrderDto>)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public OrderDto Get(int id)
    {
        lock (store.SyncRoot)
        {
            var order = store.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Order with Id {id} not found");

            return mapper.Map<OrderDto>(order);
        }
    }

    /// <inheritdoc/>
    public OrderDto Create(OrderCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var (customer, restaurant, items) = ResolveAndValidate(dto);

            var order = mapper.Map<Order>(dto);
            order.Id = store.NextOrderId();
            order.Customer = customer;
            order.Restaurant = restaurant;
            order.Items = items;

            store.Orders.Add(order);
            return mapper.Map<OrderDto>(order);
        }
    }

    /// <inheritdoc/>
    public OrderDto Update(int id, OrderCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var order = store.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Order with Id {id} not found");

            var (customer, restaurant, items) = ResolveAndValidate(dto);

            mapper.Map(dto, order);
            order.Customer = customer;
            order.Restaurant = restaurant;
            order.Items = items;

            return mapper.Map<OrderDto>(order);
        }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var order = store.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Order with Id {id} not found");

            store.Orders.Remove(order);
        }
    }

    private (Customer Customer, Restaurant Restaurant, List<OrderItem> Items) ResolveAndValidate(
        OrderCreateUpdateDto dto)
    {
        if (dto.DeliveredAt.HasValue && dto.DeliveredAt.Value < dto.CreatedAt)
        {
            throw new ArgumentException("Delivery time cannot be earlier than order creation time.");
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
                throw new ArgumentException($"Dish '{dish.Name}' does not belong to restaurant '{restaurant.Name}'.");
            }

            var orderItem = mapper.Map<OrderItem>(itemDto);
            orderItem.Id = store.NextOrderItemId();
            orderItem.Dish = dish;
            items.Add(orderItem);
        }

        return (customer, restaurant, items);
    }
}
