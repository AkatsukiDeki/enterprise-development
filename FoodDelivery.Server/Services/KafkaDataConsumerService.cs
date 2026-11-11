using System.Text.Json;
using Confluent.Kafka;
using FoodDelivery.Domain.Messages;
using FoodDelivery.Server.Data;
using Microsoft.EntityFrameworkCore;

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

        // Первичная инициализация схемы БД до входа в цикл обработки сообщений
        try
        {
            using var initScope = _serviceProvider.CreateScope();
            var initDb = initScope.ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();
            await initDb.Database.EnsureCreatedAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при первичной инициализации базы данных перед стартом Kafka-консьюмера.");
        }

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

                    // Синхронизируем справочники и сущности при их отсутствии
                    if (payload.Categories?.Count > 0 && !await db.Categories.AnyAsync(stoppingToken))
                    {
                        await db.Categories.AddRangeAsync(payload.Categories, stoppingToken);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (payload.Restaurants?.Count > 0 && !await db.Restaurants.AnyAsync(stoppingToken))
                    {
                        await db.Restaurants.AddRangeAsync(payload.Restaurants, stoppingToken);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (payload.Customers?.Count > 0 && !await db.Customers.AnyAsync(stoppingToken))
                    {
                        await db.Customers.AddRangeAsync(payload.Customers, stoppingToken);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (payload.Dishes?.Count > 0 && !await db.Dishes.AnyAsync(stoppingToken))
                    {
                        await db.Dishes.AddRangeAsync(payload.Dishes, stoppingToken);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (payload.Orders?.Count > 0 && !await db.Orders.AnyAsync(stoppingToken))
                    {
                        await db.Orders.AddRangeAsync(payload.Orders, stoppingToken);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    _logger.LogInformation("Успешно синхронизированы данные из Kafka в MS SQL Server.");
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ожидание Kafka брокера: {Message}", ex.Message);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}