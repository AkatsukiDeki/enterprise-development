using FoodDelivery.Server.Data;
using FoodDelivery.Server.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<FoodDeliveryDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("FoodDeliveryDb")
                           ?? builder.Configuration.GetConnectionString("sqlserver")
                           ?? "Server=localhost;Database=FoodDeliveryDb;Trusted_Connection=True;TrustServerCertificate=True;";
    options.UseSqlServer(connectionString);
});

builder.Services.AddHostedService<KafkaDataConsumerService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.Run();

public partial class Program { }