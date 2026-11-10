using System.Text.Json;
using Confluent.Kafka;
using FoodDelivery.Domain.Messages;

namespace FoodDelivery.DataGenerator;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Небольшая задержка для старта брокера
        await Task.Delay(3000, stoppingToken);

        var kafkaBootstrap = _configuration.GetConnectionString("kafka") ?? "localhost:9092";
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaBootstrap,
            Acks = Acks.All
        };

        using var producer = new ProducerBuilder<string, string>(producerConfig).Build();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var payload = DataFakerService.Generate();
                var json = JsonSerializer.Serialize(payload);

                _logger.LogInformation("Отправка партии данных через Kafka (заказов: {Count})...", payload.Orders.Count);

                await producer.ProduceAsync("food-delivery-sync", new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = json
                }, stoppingToken);

                _logger.LogInformation("Партия успешно отправлена в Kafka.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Ошибка отправки в Kafka: {Message}. Повтор через 10 сек.", ex.Message);
            }

            // Генерируем пачку раз в 30 секунд
            await Task.Delay(30000, stoppingToken);
        }
    }
}