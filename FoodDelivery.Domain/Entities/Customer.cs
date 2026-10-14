namespace FoodDelivery.Domain.Entities;

/// <summary>
/// Клиент службы доставки.
/// </summary>
public sealed class Customer
{
    /// <summary>
    /// Уникальный идентификатор клиента.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Фамилия клиента.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Имя клиента.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Отчество клиента (при наличии).
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Контактный номер телефона.
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Адрес доставки по умолчанию.
    /// </summary>
    public required string DeliveryAddress { get; set; }

    /// <summary>
    /// Полное имя клиента для отображения.
    /// </summary>
    public string FullName => string.IsNullOrWhiteSpace(Patronymic)
        ? $"{LastName} {FirstName}"
        : $"{LastName} {FirstName} {Patronymic}";
}