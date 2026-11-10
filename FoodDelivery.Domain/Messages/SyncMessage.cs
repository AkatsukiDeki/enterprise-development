using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Domain.Messages;

/// <summary>
/// Пакет сгенерированных данных для синхронизации через Kafka.
/// </summary>
public class SyncPayload
{
    public List<Category> Categories { get; set; } = [];
    public List<Restaurant> Restaurants { get; set; } = [];
    public List<Customer> Customers { get; set; } = [];
    public List<Dish> Dishes { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
}