using FoodDelivery.Domain.Data;
using FoodDelivery.Server.Data;
using FoodDelivery.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodDelivery.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var hostedServices = services.Where(d => d.ImplementationType == typeof(KafkaDataConsumerService)).ToList();
            foreach (var service in hostedServices)
            {
                services.Remove(service);
            }

            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<FoodDeliveryDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(FoodDeliveryDbContext) ||
                (d.ServiceType.FullName != null && d.ServiceType.FullName.StartsWith("Microsoft.EntityFrameworkCore"))
            ).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            var dbName = "TestDb_" + Guid.NewGuid();
            services.AddDbContext<FoodDeliveryDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();

            db.Database.EnsureCreated();
            SeedTestData(db);
        });
    }

    private static void SeedTestData(FoodDeliveryDbContext db)
    {
        if (!db.Categories.Any())
        {
            var testData = new TestData();
            db.Categories.AddRange(testData.Categories);
            db.Restaurants.AddRange(testData.Restaurants);
            db.Customers.AddRange(testData.Customers);
            db.Dishes.AddRange(testData.Dishes);
            db.Orders.AddRange(testData.Orders);
            db.SaveChanges();
        }
    }
}