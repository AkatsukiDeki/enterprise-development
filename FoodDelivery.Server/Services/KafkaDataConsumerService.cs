using System.Text.Json;
using Confluent.Kafka;
using FoodDelivery.Domain.Messages;
using FoodDelivery.Server.Data;

namespace FoodDelivery.Server.Services;

/// <summary>
/// Фоновый консьюмер Kafka для приема и сохранения сгенерированных данных в MS SQL Server.
/// </summary>
public class KafkaDataConsumerService : BackgroundService
{
    private readonly ILogger<KafkaDataConsumerService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public KafkaDataConsumerService(
        ILogger<KafkaDataConsumerService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        var kafkaBootstrap = _configuration.GetConnectionString("kafka") ?? "localhost:9092";
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaBootstrap,
            GroupId = "food-delivery-server-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
                consumer.Subscribe("food-delivery-sync");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(3));
                    if (result?.Message?.Value is null)
                    {
                        continue;
                    }

                    var payload = JsonSerializer.Deserialize<SyncPayload>(result.Message.Value);
                    if (payload is null)
                    {
                        continue;
                    }

                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();

                    await db.Database.EnsureCreatedAsync(stoppingToken);

                    // Синхронизируем справочники и сущности при их отсутствии
                    if (!db.Categories.Any())
                    {
                        db.Categories.AddRange(payload.Categories);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (!db.Restaurants.Any())
                    {
                        db.Restaurants.AddRange(payload.Restaurants);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (!db.Customers.Any())
                    {
                        db.Customers.AddRange(payload.Customers);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (!db.Dishes.Any())
                    {
                        db.Dishes.AddRange(payload.Dishes);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (!db.Orders.Any())
                    {
                        db.Orders.AddRange(payload.Orders);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    _logger.LogInformation("Успешно синхронизированы данные из Kafka в MS SQL Server.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ожидание Kafka брокера: {Message}", ex.Message);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}