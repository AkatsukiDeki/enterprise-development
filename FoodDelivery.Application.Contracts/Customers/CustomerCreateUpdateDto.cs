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
    [Required] [MinLength(1)] string LastName,
    [Required] [MinLength(1)] string FirstName,
    string? Patronymic,
    [Required] [MinLength(1)] string PhoneNumber,
    [Required] [MinLength(1)] string DeliveryAddress);
