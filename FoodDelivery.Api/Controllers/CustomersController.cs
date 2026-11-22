using FoodDelivery.Application.Contracts.Customers;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Контроллер управления клиентами.
/// </summary>
/// <param name="service">Сервис клиентов.</param>
public sealed class CustomersController(ICustomerService service)
    : CrudControllerBase<CustomerDto, CustomerCreateUpdateDto, ICustomerService>(service);
