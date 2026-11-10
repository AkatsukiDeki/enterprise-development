using Bogus;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Domain.Messages;

namespace FoodDelivery.DataGenerator;

/// <summary>
/// Сервис генерации реалистичных доменных данных с помощью Bogus.
/// </summary>
public static class DataFakerService
{
    public static SyncPayload Generate()
    {
        var categoryNames = new[]
        {
            "Пицца", "Суши", "Бургеры", "Супы", "Салаты",
            "Паста", "Десерты", "Напитки", "Завтраки", "Гриль"
        };

        var categories = categoryNames
            .Select((name, i) => new Category { Id = i + 1, Name = name })
            .ToList();

        var customerFaker = new Faker<Customer>("ru")
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.Patronymic, f => f.Random.Bool(0.85f) ? f.Name.FirstName() + "ович" : null)
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("+79#########"))
            .RuleFor(c => c.DeliveryAddress, f => f.Address.StreetAddress());

        var customers = customerFaker.Generate(15);

        var restaurantNames = new[]
        {
            "Додо", "Тануки", "Бургер Клаб", "Перчини", "Вкусно и точка",
            "СушиВесла", "МясоRoob", "Шоколадница", "ФоБо", "Папа Джонс"
        };

        var restaurantFaker = new Faker<Restaurant>("ru")
            .RuleFor(r => r.Id, f => f.IndexFaker + 1)
            .RuleFor(r => r.Name, f => restaurantNames[f.IndexFaker % restaurantNames.Length])
            .RuleFor(r => r.Address, f => f.Address.StreetAddress())
            .RuleFor(r => r.Rating, f => Math.Round(f.Random.Double(3.8, 5.0), 1))
            .RuleFor(r => r.OpeningTime, f => new TimeOnly(8, 0))
            .RuleFor(r => r.ClosingTime, f => new TimeOnly(23, 0));

        var restaurants = restaurantFaker.Generate(10);

        var dishFaker = new Faker<Dish>("ru")
            .RuleFor(d => d.Id, f => f.IndexFaker + 1)
            .RuleFor(d => d.Name, f => f.Commerce.ProductName())
            .RuleFor(d => d.WeightGrams, f => f.Random.Int(150, 750))
            .RuleFor(d => d.Price, f => f.Random.Decimal(190, 1400))
            .RuleFor(d => d.Category, f => f.PickRandom(categories))
            .RuleFor(d => d.Restaurant, f => f.PickRandom(restaurants));

        var dishes = dishFaker.Generate(30);

        var orderItemId = 1;
        var orderFaker = new Faker<Order>("ru")
            .RuleFor(o => o.Id, f => f.IndexFaker + 1)
            .RuleFor(o => o.Customer, f => f.PickRandom(customers))
            .RuleFor(o => o.Restaurant, f => f.PickRandom(restaurants))
            // Даты распределяем с запасом до и после сентября для проверки фильтра
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(new DateTime(2026, 8, 20), new DateTime(2026, 10, 10)))
            .RuleFor(o => o.DeliveredAt, (f, o) => f.Random.Bool(0.9f) ? o.CreatedAt.AddMinutes(f.Random.Int(20, 90)) : null)
            .RuleFor(o => o.Items, (f, o) =>
            {
                var restaurantDishes = dishes.Where(d => d.Restaurant.Id == o.Restaurant.Id).ToList();
                if (restaurantDishes.Count == 0)
                {
                    restaurantDishes = [dishes.First(d => d.Restaurant.Id == o.Restaurant.Id)];
                }

                return restaurantDishes
                    .Take(f.Random.Int(1, Math.Min(3, restaurantDishes.Count)))
                    .Select(d => new OrderItem
                    {
                        Id = orderItemId++,
                        Dish = d,
                        Quantity = f.Random.Int(1, 3)
                    })
                    .ToList();
            });

        var orders = orderFaker.Generate(25);

        return new SyncPayload
        {
            Categories = categories,
            Restaurants = restaurants,
            Customers = customers,
            Dishes = dishes,
            Orders = orders
        };
    }
}