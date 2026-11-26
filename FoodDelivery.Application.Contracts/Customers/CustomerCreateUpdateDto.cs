using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Application.Contracts.Customers;

/// <summary>
/// DTO для создания или обновления клиента.
/// </summary>
/// <param name="LastName">Фамилия клиента.</param>
/// <param name="FirstName">Имя клиента.</param>
/// <param name="Patronymic">Отчество клиента (при наличии).</param>
/// <param name="PhoneNumber">Контактный номер телефона.</param>
/// <param name="DeliveryAddress">Адрес доставки.</param>
public sealed record CustomerCreateUpdateDto(
    [Required] string LastName,
    [Required] string FirstName,
    string? Patronymic,
    [Required] string PhoneNumber,
    [Required] string DeliveryAddress);
