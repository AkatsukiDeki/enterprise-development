namespace FoodDelivery.Application.Contracts.Customers;

/// <summary>
/// DTO клиента службы доставки.
/// </summary>
/// <param name="Id">Уникальный идентификатор клиента.</param>
/// <param name="LastName">Фамилия клиента.</param>
/// <param name="FirstName">Имя клиента.</param>
/// <param name="Patronymic">Отчество клиента.</param>
/// <param name="FullName">Полное имя клиента.</param>
/// <param name="PhoneNumber">Контактный номер телефона.</param>
/// <param name="DeliveryAddress">Адрес доставки.</param>
public sealed record CustomerDto(
    int Id,
    string LastName,
    string FirstName,
    string? Patronymic,
    string FullName,
    string PhoneNumber,
    string DeliveryAddress) : IHasId<int>;
