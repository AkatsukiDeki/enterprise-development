var builder = DistributedApplication.CreateBuilder(args);

// 1. Контейнер MS SQL Server с базой FoodDeliveryDb
var sqlServer = builder.AddSqlServer("sqlserver")
    .AddDatabase("FoodDeliveryDb");

// 2. Брокер сообщений Apache Kafka
var kafka = builder.AddKafka("kafka");

// 3. Сервер REST API (подключаем SQL и Kafka)
var server = builder.AddProject<Projects.FoodDelivery_Server>("server")
    .WithReference(sqlServer)
    .WithReference(kafka);

// 4. Генератор данных Bogus (подключаем к Kafka, запускаем после сервера)
builder.AddProject<Projects.FoodDelivery_DataGenerator>("generator")
    .WithReference(kafka)
    .WaitFor(server);

builder.Build().Run();