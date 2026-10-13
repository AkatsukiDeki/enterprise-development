using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Domain.Data;

/// <summary>
/// Тестовый набор данных со связанными сущностями.
/// </summary>
public static class TestData
{
    /// <summary>Категории блюд.</summary>
    public static readonly List<Category> Categories =
    [
        new() { Id = 1, Name = "Пицца" },
        new() { Id = 2, Name = "Суши" },
        new() { Id = 3, Name = "Бургеры" },
        new() { Id = 4, Name = "Супы" },
        new() { Id = 5, Name = "Салаты" },
        new() { Id = 6, Name = "Паста" },
        new() { Id = 7, Name = "Десерты" },
        new() { Id = 8, Name = "Напитки" },
        new() { Id = 9, Name = "Завтраки" },
        new() { Id = 10, Name = "Гриль" }
    ];

    /// <summary>Рестораны.</summary>
    public static readonly List<Restaurant> Restaurants =
    [
        new() { Id = 1, Name = "Додо", Address = "ул. Победы, 10", Rating = 4.7, OpeningTime = new TimeOnly(8, 0), ClosingTime = new TimeOnly(23, 0) },
        new() { Id = 2, Name = "Тануки", Address = "ул. Ленина, 25", Rating = 4.8, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(23, 30) },
        new() { Id = 3, Name = "Бургер Клаб", Address = "пр. Юных Пионеров, 1", Rating = 4.2, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(22, 0) },
        new() { Id = 4, Name = "Перчини", Address = "ул. Садовая, 5", Rating = 4.9, OpeningTime = new TimeOnly(11, 0), ClosingTime = new TimeOnly(23, 0) },
        new() { Id = 5, Name = "Вкусно и точка", Address = "Московское шоссе, 15", Rating = 4.5, OpeningTime = new TimeOnly(7, 0), ClosingTime = new TimeOnly(0, 0) },
        new() { Id = 6, Name = "СушиВесла", Address = "ул. Гагарина, 80", Rating = 4.1, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(22, 0) },
        new() { Id = 7, Name = "МясоRoob", Address = "ул. Куйбышева, 4", Rating = 4.6, OpeningTime = new TimeOnly(12, 0), ClosingTime = new TimeOnly(23, 0) },
        new() { Id = 8, Name = "Шоколадница", Address = "ул. Полевая, 12", Rating = 4.3, OpeningTime = new TimeOnly(8, 30), ClosingTime = new TimeOnly(21, 0) },
        new() { Id = 9, Name = "ФоБо", Address = "ул. Стара-Загора, 100", Rating = 4.4, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(22, 0) },
        new() { Id = 10, Name = "Папа Джонс", Address = "ул. Советской Армии, 30", Rating = 4.6, OpeningTime = new TimeOnly(9, 0), ClosingTime = new TimeOnly(23, 0) }
    ];

    /// <summary>Клиенты.</summary>
    public static readonly List<Customer> Customers =
    [
        new() { Id = 1, LastName = "Иванов", FirstName = "Иван", PhoneNumber = "+79001110001", DeliveryAddress = "ул. Мира, 1" },
        new() { Id = 2, LastName = "Петров", FirstName = "Петр", PhoneNumber = "+79001110002", DeliveryAddress = "ул. Садовая, 2" },
        new() { Id = 3, LastName = "Сидоров", FirstName = "Алексей", PhoneNumber = "+79001110003", DeliveryAddress = "ул. Ленина, 3" },
        new() { Id = 4, LastName = "Смирнов", FirstName = "Дмитрий", PhoneNumber = "+79001110004", DeliveryAddress = "ул. Гагарина, 4" },
        new() { Id = 5, LastName = "Кузнецов", FirstName = "Сергей", PhoneNumber = "+79001110005", DeliveryAddress = "ул. Победы, 5" },
        new() { Id = 6, LastName = "Попов", FirstName = "Михаил", PhoneNumber = "+79001110006", DeliveryAddress = "ул. Полевая, 6" },
        new() { Id = 7, LastName = "Васильев", FirstName = "Андрей", PhoneNumber = "+79001110007", DeliveryAddress = "ул. Киевская, 7" },
        new() { Id = 8, LastName = "Павлов", FirstName = "Николай", PhoneNumber = "+79001110008", DeliveryAddress = "ул. Ново-Садовая, 8" },
        new() { Id = 9, LastName = "Семенов", FirstName = "Артем", PhoneNumber = "+79001110009", DeliveryAddress = "ул. Советская, 9" },
        new() { Id = 10, LastName = "Федоров", FirstName = "Владимир", PhoneNumber = "+79001110010", DeliveryAddress = "ул. Дачная, 10" }
    ];

    /// <summary>Блюда ресторанов.</summary>
    public static readonly List<Dish> Dishes =
    [
        new() { Id = 1, Name = "Пепперони", WeightGrams = 450, Price = 550m, CategoryId = 1, Category = Categories[0], RestaurantId = 1, Restaurant = Restaurants[0] },
        new() { Id = 2, Name = "Филадельфия", WeightGrams = 280, Price = 620m, CategoryId = 2, Category = Categories[1], RestaurantId = 2, Restaurant = Restaurants[1] },
        new() { Id = 3, Name = "Чизбургер Премиум", WeightGrams = 320, Price = 420m, CategoryId = 3, Category = Categories[2], RestaurantId = 3, Restaurant = Restaurants[2] },
        new() { Id = 4, Name = "Том Ям", WeightGrams = 400, Price = 490m, CategoryId = 4, Category = Categories[3], RestaurantId = 9, Restaurant = Restaurants[8] },
        new() { Id = 5, Name = "Цезарь с курицей", WeightGrams = 220, Price = 380m, CategoryId = 5, Category = Categories[4], RestaurantId = 4, Restaurant = Restaurants[3] },
        new() { Id = 6, Name = "Карбонара", WeightGrams = 300, Price = 470m, CategoryId = 6, Category = Categories[5], RestaurantId = 4, Restaurant = Restaurants[3] },
        new() { Id = 7, Name = "Чизкейк Нью-Йорк", WeightGrams = 150, Price = 290m, CategoryId = 7, Category = Categories[6], RestaurantId = 8, Restaurant = Restaurants[7] },
        new() { Id = 8, Name = "Капучино", WeightGrams = 300, Price = 210m, CategoryId = 8, Category = Categories[7], RestaurantId = 8, Restaurant = Restaurants[7] },
        new() { Id = 9, Name = "Сырники со сметаной", WeightGrams = 200, Price = 310m, CategoryId = 9, Category = Categories[8], RestaurantId = 8, Restaurant = Restaurants[7] },
        new() { Id = 10, Name = "Стейк Рибай", WeightGrams = 350, Price = 1450m, CategoryId = 10, Category = Categories[9], RestaurantId = 7, Restaurant = Restaurants[6] }
    ];

    /// <summary>Заказы.</summary>
    public static readonly List<Order> Orders =
    [
        // Ресторан 7 (3 заказа)
        new()
        {
            Id = 1,
            CustomerId = 1, Customer = Customers[0],
            RestaurantId = 7, Restaurant = Restaurants[6],
            CreatedAt = new DateTime(2026, 9, 1, 12, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 1, 12, 40, 0),
            Items = [ new() { Id = 1, DishId = 10, Dish = Dishes[9], Quantity = 2 } ]
        },
        new()
        {
            Id = 2,
            CustomerId = 2, Customer = Customers[1],
            RestaurantId = 7, Restaurant = Restaurants[6],
            CreatedAt = new DateTime(2026, 9, 2, 13, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 2, 13, 35, 0),
            Items = [ new() { Id = 2, DishId = 10, Dish = Dishes[9], Quantity = 1 } ]
        },
        new()
        {
            Id = 3,
            CustomerId = 1, Customer = Customers[0],
            RestaurantId = 7, Restaurant = Restaurants[6],
            CreatedAt = new DateTime(2026, 9, 3, 19, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 3, 19, 50, 0),
            Items = [ new() { Id = 3, DishId = 10, Dish = Dishes[9], Quantity = 1 } ]
        },

        // Ресторан 2 (4 заказа - лидер)
        new()
        {
            Id = 4,
            CustomerId = 3, Customer = Customers[2],
            RestaurantId = 2, Restaurant = Restaurants[1],
            CreatedAt = new DateTime(2026, 9, 4, 14, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 4, 14, 25, 0),
            Items = [ new() { Id = 4, DishId = 2, Dish = Dishes[1], Quantity = 2 } ]
        },
        new()
        {
            Id = 5,
            CustomerId = 4, Customer = Customers[3],
            RestaurantId = 2, Restaurant = Restaurants[1],
            CreatedAt = new DateTime(2026, 9, 5, 15, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 5, 15, 30, 0),
            Items = [ new() { Id = 5, DishId = 2, Dish = Dishes[1], Quantity = 1 } ]
        },
        new()
        {
            Id = 6,
            CustomerId = 1, Customer = Customers[0],
            RestaurantId = 2, Restaurant = Restaurants[1],
            CreatedAt = new DateTime(2026, 9, 6, 18, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 6, 18, 20, 0),
            Items = [ new() { Id = 6, DishId = 2, Dish = Dishes[1], Quantity = 3 } ]
        },
        new()
        {
            Id = 7,
            CustomerId = 3, Customer = Customers[2],
            RestaurantId = 2, Restaurant = Restaurants[1],
            CreatedAt = new DateTime(2026, 9, 7, 20, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 7, 20, 35, 0),
            Items = [ new() { Id = 7, DishId = 2, Dish = Dishes[1], Quantity = 1 } ]
        },

        // Ресторан 4 (3 заказа)
        new()
        {
            Id = 8,
            CustomerId = 5, Customer = Customers[4],
            RestaurantId = 4, Restaurant = Restaurants[3],
            CreatedAt = new DateTime(2026, 9, 8, 12, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 8, 12, 30, 0),
            Items = [ new() { Id = 8, DishId = 5, Dish = Dishes[4], Quantity = 2 } ]
        },
        new()
        {
            Id = 9,
            CustomerId = 6, Customer = Customers[5],
            RestaurantId = 4, Restaurant = Restaurants[3],
            CreatedAt = new DateTime(2026, 9, 9, 13, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 9, 13, 40, 0),
            Items = [ new() { Id = 9, DishId = 6, Dish = Dishes[5], Quantity = 2 } ]
        },
        new()
        {
            Id = 10,
            CustomerId = 5, Customer = Customers[4],
            RestaurantId = 4, Restaurant = Restaurants[3],
            CreatedAt = new DateTime(2026, 9, 10, 14, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 10, 14, 30, 0),
            Items = [ new() { Id = 10, DishId = 6, Dish = Dishes[5], Quantity = 1 } ]
        },

        // Ресторан 1 (2 заказа)
        new()
        {
            Id = 11,
            CustomerId = 7, Customer = Customers[6],
            RestaurantId = 1, Restaurant = Restaurants[0],
            CreatedAt = new DateTime(2026, 9, 11, 11, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 11, 11, 15, 0), // 15 мин
            Items = [ new() { Id = 11, DishId = 1, Dish = Dishes[0], Quantity = 2 } ]
        },
        new()
        {
            Id = 12,
            CustomerId = 8, Customer = Customers[7],
            RestaurantId = 1, Restaurant = Restaurants[0],
            CreatedAt = new DateTime(2026, 9, 12, 12, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 12, 12, 35, 0),
            Items = [ new() { Id = 12, DishId = 1, Dish = Dishes[0], Quantity = 1 } ]
        },

        // Ресторан 8 (2 заказа)
        new()
        {
            Id = 13,
            CustomerId = 9, Customer = Customers[8],
            RestaurantId = 8, Restaurant = Restaurants[7],
            CreatedAt = new DateTime(2026, 9, 13, 10, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 13, 10, 30, 0),
            Items = [ new() { Id = 13, DishId = 7, Dish = Dishes[6], Quantity = 2 } ]
        },
        new()
        {
            Id = 14,
            CustomerId = 10, Customer = Customers[9],
            RestaurantId = 8, Restaurant = Restaurants[7],
            CreatedAt = new DateTime(2026, 9, 14, 11, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 14, 11, 35, 0),
            Items = [ new() { Id = 14, DishId = 8, Dish = Dishes[7], Quantity = 3 } ]
        },

        // Ресторан 3 (1 заказ)
        new()
        {
            Id = 15,
            CustomerId = 2, Customer = Customers[1],
            RestaurantId = 3, Restaurant = Restaurants[2],
            CreatedAt = new DateTime(2026, 9, 15, 17, 0, 0),
            DeliveredAt = new DateTime(2026, 9, 15, 17, 30, 0),
            Items = [ new() { Id = 15, DishId = 3, Dish = Dishes[2], Quantity = 2 } ]
        }
    ];
}