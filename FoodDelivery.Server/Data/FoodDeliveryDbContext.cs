using FoodDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Data;

/// <summary>
/// Контекст базы данных службы доставки еды (MS SQL Server).
/// </summary>
public class FoodDeliveryDbContext : DbContext
{
    public FoodDeliveryDbContext(DbContextOptions<FoodDeliveryDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Игнорируем вычисляемые свойства доменных моделей
        modelBuilder.Entity<Customer>()
            .Ignore(c => c.FullName);

        modelBuilder.Entity<Order>()
            .Ignore(o => o.TotalAmount)
            .Ignore(o => o.DeliveryDuration);

        modelBuilder.Entity<OrderItem>()
            .Ignore(i => i.SubTotal);

        // Настройка точности денежных полей для MS SQL Server
        modelBuilder.Entity<Dish>()
            .Property(d => d.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Rating)
            .HasPrecision(3, 1);
    }
}