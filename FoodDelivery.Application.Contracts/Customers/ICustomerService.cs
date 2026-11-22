namespace FoodDelivery.Application.Contracts.Customers;

/// <summary>
/// Контракт сервиса для работы с клиентами.
/// </summary>
public interface ICustomerService : IApplicationService<CustomerDto, CustomerCreateUpdateDto, int>;
