using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Domain.Data;

/// <summary>
/// Набор тестовых данных для юнит-тестов (xUnit Class Fixture).
/// </summary>
public class TestData
{
    /// <summary>
    /// Категории блюд.
    /// </summary>
    public List<Category> Categories { get; }

    /// <summary>
    /// Рестораны.
    /// </summary>
    public List<Restaurant> Restaurants { get; }

    /// <summary>
    /// Клиенты.
    /// </summary>
    public List<Customer> Customers { get; }

    /// <summary>
    /// Меню блюд.
    /// </summary>
    public List<Dish> Dishes { get; }

    /// <summary>
    /// Заказы.
    /// </summary>
    public List<Order> Orders { get; }

    /// <summary>
    /// Инициализация тестовых данных.
    /// </summary>
    public TestData()
    {
        Categories =
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

        Restaurants =
        [
            new() { Id = 1, Name = "Додо", Address = "ул. Победы, 10", Rating = 4.7, OpeningTime = new TimeOnly(8, 0), ClosingTime = new TimeOnly(23, 0) },
            new() { Id = 2, Name = "Тануки", Address = "ул. Ленина, 25", Rating = 4.8, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(23, 30) },
            new() { Id = 3, Name = "Бургер Клаб", Address = "пр. Юных Пионеров, 1", Rating = 4.2, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(22, 0) },
            new() { Id = 4, Name = "Перчини", Address = "ул. Садовая, 5", Rating = 4.9, OpeningTime = new TimeOnly(11, 0), ClosingTime = new TimeOnly(23, 0) },
            new() { Id = 5, Name = "Вкусно и точка", Address = "Московское шоссе, 15", Rating = 4.5, OpeningTime = new TimeOnly(7, 0), ClosingTime = new TimeOnly(23, 59) },
            new() { Id = 6, Name = "СушиВесла", Address = "ул. Гагарина, 80", Rating = 4.1, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(22, 0) },
            new() { Id = 7, Name = "МясоRoob", Address = "ул. Куйбышева, 4", Rating = 4.6, OpeningTime = new TimeOnly(12, 0), ClosingTime = new TimeOnly(23, 0) },
            new() { Id = 8, Name = "Шоколадница", Address = "ул. Полевая, 12", Rating = 4.3, OpeningTime = new TimeOnly(8, 30), ClosingTime = new TimeOnly(21, 0) },
            new() { Id = 9, Name = "ФоБо", Address = "ул. Стара-Загора, 100", Rating = 4.4, OpeningTime = new TimeOnly(10, 0), ClosingTime = new TimeOnly(22, 0) },
            new() { Id = 10, Name = "Папа Джонс", Address = "ул. Советской Армии, 30", Rating = 4.6, OpeningTime = new TimeOnly(9, 0), ClosingTime = new TimeOnly(23, 0) }
        ];

        Customers =
        [
            new() { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", PhoneNumber = "+79001110001", DeliveryAddress = "ул. Мира, 1" },
            new() { Id = 2, LastName = "Петров", FirstName = "Петр", Patronymic = "Сергеевич", PhoneNumber = "+79001110002", DeliveryAddress = "ул. Садовая, 2" },
            new() { Id = 3, LastName = "Сидоров", FirstName = "Алексей", Patronymic = "Михайлович", PhoneNumber = "+79001110003", DeliveryAddress = "ул. Ленина, 3" },
            new() { Id = 4, LastName = "Смирнов", FirstName = "Дмитрий", Patronymic = "Александрович", PhoneNumber = "+79001110004", DeliveryAddress = "ул. Гагарина, 4" },
            new() { Id = 5, LastName = "Кузнецов", FirstName = "Сергей", Patronymic = "Павлович", PhoneNumber = "+79001110005", DeliveryAddress = "ул. Победы, 5" },
            new() { Id = 6, LastName = "Попов", FirstName = "Михаил", Patronymic = "Андреевич", PhoneNumber = "+79001110006", DeliveryAddress = "ул. Полевая, 6" },
            new() { Id = 7, LastName = "Васильев", FirstName = "Андрей", Patronymic = "Николаевич", PhoneNumber = "+79001110007", DeliveryAddress = "ул. Киевская, 7" },
            new() { Id = 8, LastName = "Павлов", FirstName = "Николай", Patronymic = null, PhoneNumber = "+79001110008", DeliveryAddress = "ул. Ново-Садовая, 8" },
            new() { Id = 9, LastName = "Семенов", FirstName = "Артем", Patronymic = "Олегович", PhoneNumber = "+79001110009", DeliveryAddress = "ул. Советская, 9" },
            new() { Id = 10, LastName = "Федоров", FirstName = "Владимир", Patronymic = "Дмитриевич", PhoneNumber = "+79001110010", DeliveryAddress = "ул. Дачная, 10" }
        ];

        Dishes =
        [
            // Додо (Ресторан 1)
            new() { Id = 1, Name = "Пепперони", WeightGrams = 450, Price = 550m, Category = Categories[0], Restaurant = Restaurants[0] },
            new() { Id = 2, Name = "Морс Клюква", WeightGrams = 500, Price = 150m, Category = Categories[7], Restaurant = Restaurants[0] },

            // Тануки (Ресторан 2)
            new() { Id = 3, Name = "Филадельфия", WeightGrams = 280, Price = 620m, Category = Categories[1], Restaurant = Restaurants[1] },
            new() { Id = 4, Name = "Калифорния", WeightGrams = 260, Price = 540m, Category = Categories[1], Restaurant = Restaurants[1] },
            new() { Id = 5, Name = "Зеленый чай", WeightGrams = 400, Price = 120m, Category = Categories[7], Restaurant = Restaurants[1] },

            // Бургер Клаб (Ресторан 3)
            new() { Id = 6, Name = "Чизбургер Премиум", WeightGrams = 320, Price = 420m, Category = Categories[2], Restaurant = Restaurants[2] },

            // Перчини (Ресторан 4)
            new() { Id = 7, Name = "Цезарь с курицей", WeightGrams = 220, Price = 380m, Category = Categories[4], Restaurant = Restaurants[3] },
            new() { Id = 8, Name = "Карбонара", WeightGrams = 300, Price = 470m, Category = Categories[5], Restaurant = Restaurants[3] },

            // Вкусно и точка (Ресторан 5)
            new() { Id = 9, Name = "Гранд Де Люкс", WeightGrams = 250, Price = 230m, Category = Categories[2], Restaurant = Restaurants[4] },

            // СушиВесла (Ресторан 6)
            new() { Id = 10, Name = "Запеченный с лососем", WeightGrams = 290, Price = 460m, Category = Categories[1], Restaurant = Restaurants[5] },

            // МясоRoob (Ресторан 7)
            new() { Id = 11, Name = "Стейк Рибай", WeightGrams = 350, Price = 1450m, Category = Categories[9], Restaurant = Restaurants[6] },
            new() { Id = 12, Name = "Овощи гриль", WeightGrams = 200, Price = 320m, Category = Categories[9], Restaurant = Restaurants[6] },

            // Шоколадница (Ресторан 8)
            new() { Id = 13, Name = "Чизкейк Нью-Йорк", WeightGrams = 150, Price = 290m, Category = Categories[6], Restaurant = Restaurants[7] },
            new() { Id = 14, Name = "Капучино", WeightGrams = 300, Price = 210m, Category = Categories[7], Restaurant = Restaurants[7] },

            // ФоБо (Ресторан 9)
            new() { Id = 15, Name = "Суп Фо Бо", WeightGrams = 600, Price = 490m, Category = Categories[3], Restaurant = Restaurants[8] },

            // Папа Джонс (Ресторан 10)
            new() { Id = 16, Name = "Супер Папа", WeightGrams = 520, Price = 690m, Category = Categories[0], Restaurant = Restaurants[9] }
        ];

        Orders =
        [
            // Ресторан 7: МясоRoob (3 заказа)
            new()
            {
                Id = 1,
                Customer = Customers[0], // Иванов
                Restaurant = Restaurants[6],
                CreatedAt = new DateTime(2026, 9, 1, 12, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 1, 12, 40, 0),
                Items =
                [
                    new() { Id = 1, Dish = Dishes[10], Quantity = 2 }, // Гриль: 2x1450 = 2900
                    new() { Id = 2, Dish = Dishes[11], Quantity = 1 }  // Гриль: 1x320 = 320 -> Заказ: 3220
                ]
            },
            new()
            {
                Id = 2,
                Customer = Customers[1], // Петров
                Restaurant = Restaurants[6],
                CreatedAt = new DateTime(2026, 9, 2, 13, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 2, 13, 35, 0),
                Items = [ new() { Id = 3, Dish = Dishes[10], Quantity = 1 } ] // Гриль: 1450
            },
            new()
            {
                Id = 3,
                Customer = Customers[0], // Иванов
                Restaurant = Restaurants[6],
                CreatedAt = new DateTime(2026, 9, 3, 19, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 3, 19, 50, 0),
                Items = [ new() { Id = 4, Dish = Dishes[11], Quantity = 3 } ] // Гриль: 3x320 = 960
            },

            // Ресторан 2: Тануки (4 заказа)
            new()
            {
                Id = 4,
                Customer = Customers[2], // Сидоров
                Restaurant = Restaurants[1],
                CreatedAt = new DateTime(2026, 9, 4, 14, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 4, 14, 25, 0),
                Items =
                [
                    new() { Id = 5, Dish = Dishes[2], Quantity = 2 }, // Суши: 2x620 = 1240
                    new() { Id = 6, Dish = Dishes[4], Quantity = 1 }  // Напитки: 1x120 = 120
                ]
            },
            new()
            {
                Id = 5,
                Customer = Customers[3], // Смирнов
                Restaurant = Restaurants[1],
                CreatedAt = new DateTime(2026, 9, 5, 15, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 5, 15, 30, 0),
                Items = [ new() { Id = 7, Dish = Dishes[2], Quantity = 1 } ] // Суши: 620
            },
            new()
            {
                Id = 6,
                Customer = Customers[0], // Иванов
                Restaurant = Restaurants[1],
                CreatedAt = new DateTime(2026, 9, 6, 18, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 6, 18, 20, 0),
                Items =
                [
                    new() { Id = 8, Dish = Dishes[2], Quantity = 2 }, // Суши: 2x620 = 1240
                    new() { Id = 9, Dish = Dishes[3], Quantity = 1 }  // Суши: 1x540 = 540 -> Суши: 1780
                ]
            },
            new()
            {
                Id = 7,
                Customer = Customers[2], // Сидоров
                Restaurant = Restaurants[1],
                CreatedAt = new DateTime(2026, 9, 7, 20, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 7, 20, 35, 0),
                Items = [ new() { Id = 10, Dish = Dishes[3], Quantity = 2 } ] // Суши: 2x540 = 1080
            },

            // Ресторан 4: Перчини (3 заказа)
            new()
            {
                Id = 8,
                Customer = Customers[4],
                Restaurant = Restaurants[3],
                CreatedAt = new DateTime(2026, 9, 8, 12, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 8, 12, 30, 0),
                Items = [ new() { Id = 11, Dish = Dishes[6], Quantity = 2 } ] // 760
            },
            new()
            {
                Id = 9,
                Customer = Customers[5],
                Restaurant = Restaurants[3],
                CreatedAt = new DateTime(2026, 9, 9, 13, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 9, 13, 40, 0),
                Items = [ new() { Id = 12, Dish = Dishes[7], Quantity = 2 } ] // 940
            },
            new()
            {
                Id = 10,
                Customer = Customers[4],
                Restaurant = Restaurants[3],
                CreatedAt = new DateTime(2026, 9, 10, 14, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 10, 14, 30, 0),
                Items = [ new() { Id = 13, Dish = Dishes[7], Quantity = 1 } ] // 470
            },

            // Ресторан 1: Додо (2 заказа)
            new()
            {
                Id = 11,
                Customer = Customers[6],
                Restaurant = Restaurants[0],
                CreatedAt = new DateTime(2026, 9, 11, 11, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 11, 11, 15, 0), // Минимальное время: 15 минут
                Items =
                [
                    new() { Id = 14, Dish = Dishes[0], Quantity = 1 }, // Пицца: 550
                    new() { Id = 15, Dish = Dishes[1], Quantity = 2 }  // Напитки: 300
                ]
            },
            new()
            {
                Id = 12,
                Customer = Customers[7],
                Restaurant = Restaurants[0],
                CreatedAt = new DateTime(2026, 9, 12, 12, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 12, 12, 35, 0),
                Items = [ new() { Id = 16, Dish = Dishes[0], Quantity = 1 } ] // Пицца: 550
            },

            // Ресторан 8: Шоколадница (2 заказа)
            new()
            {
                Id = 13,
                Customer = Customers[8],
                Restaurant = Restaurants[7],
                CreatedAt = new DateTime(2026, 9, 13, 10, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 13, 10, 30, 0),
                Items = [ new() { Id = 17, Dish = Dishes[12], Quantity = 2 } ]
            },
            new()
            {
                Id = 14,
                Customer = Customers[9],
                Restaurant = Restaurants[7],
                CreatedAt = new DateTime(2026, 9, 14, 11, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 14, 11, 35, 0),
                Items = [ new() { Id = 18, Dish = Dishes[13], Quantity = 3 } ]
            },

            // Ресторан 3: Бургер Клаб (1 заказ)
            new()
            {
                Id = 15,
                Customer = Customers[1],
                Restaurant = Restaurants[2],
                CreatedAt = new DateTime(2026, 9, 15, 17, 0, 0),
                DeliveredAt = new DateTime(2026, 9, 15, 17, 30, 0),
                Items = [ new() { Id = 19, Dish = Dishes[5], Quantity = 2 } ]
            },

            // Заказ в пути
            new()
            {
                Id = 16,
                Customer = Customers[3],
                Restaurant = Restaurants[4], // Вкусно и точка
                CreatedAt = new DateTime(2026, 9, 16, 12, 0, 0),
                DeliveredAt = null,
                Items = [ new() { Id = 20, Dish = Dishes[8], Quantity = 1 } ]
            }
        ];
    }
}