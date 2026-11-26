using FoodDelivery.Application.Contracts.Customers;
using FoodDelivery.Domain.Entities;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления клиентами.
/// </summary>
/// <param name="store">Хранилище данных.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class CustomerService(IFoodDeliveryStore store, IMapper mapper) : ICustomerService
{
    /// <inheritdoc/>
    public IReadOnlyList<CustomerDto> GetAll()
    {
        lock (store.SyncRoot)
        {
            return store.Customers
                .Select(mapper.Map<CustomerDto>)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public CustomerDto Get(int id)
    {
        lock (store.SyncRoot)
        {
            var customer = store.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Customer with Id {id} not found");

            return mapper.Map<CustomerDto>(customer);
        }
    }

    /// <inheritdoc/>
    public CustomerDto Create(CustomerCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var customer = mapper.Map<Customer>(dto);
            customer.Id = store.NextCustomerId();

            store.Customers.Add(customer);
            return mapper.Map<CustomerDto>(customer);
        }
    }

    /// <inheritdoc/>
    public CustomerDto Update(int id, CustomerCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var customer = store.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Customer with Id {id} not found");

            mapper.Map(dto, customer);
            return mapper.Map<CustomerDto>(customer);
        }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var customer = store.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Customer with Id {id} not found");

            if (store.Orders.Any(o => o.Customer.Id == id))
            {
                throw new InvalidOperationException($"Customer with Id {id} is referenced by existing orders and cannot be deleted.");
            }

            store.Customers.Remove(customer);
        }
    }
}
