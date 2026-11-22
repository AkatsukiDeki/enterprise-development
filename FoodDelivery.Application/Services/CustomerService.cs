using FoodDelivery.Application.Contracts.Customers;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Infrastructure.InMemory;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления клиентами.
/// </summary>
/// <param name="store">Хранилище данных в памяти.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class CustomerService(InMemoryFoodDeliveryStore store, IMapper mapper) : ICustomerService
{
    /// <inheritdoc/>
    public Task<IReadOnlyList<CustomerDto>> GetAll()
    {
        lock (store.SyncRoot)
        {
            IReadOnlyList<CustomerDto> result = store.Customers
                .Select(mapper.Map<CustomerDto>)
                .ToList();
            return Task.FromResult(result);
        }
    }

    /// <inheritdoc/>
    public Task<CustomerDto> Get(int id)
    {
        lock (store.SyncRoot)
        {
            var customer = store.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Customer with Id {id} not found");

            return Task.FromResult(mapper.Map<CustomerDto>(customer));
        }
    }

    /// <inheritdoc/>
    public Task<CustomerDto> Create(CustomerCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var customer = mapper.Map<Customer>(dto);
            customer.Id = store.NextCustomerId();

            store.Customers.Add(customer);
            return Task.FromResult(mapper.Map<CustomerDto>(customer));
        }
    }

    /// <inheritdoc/>
    public Task<CustomerDto> Update(int id, CustomerCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var index = store.Customers.FindIndex(c => c.Id == id);
            if (index < 0)
            {
                throw new KeyNotFoundException($"Customer with Id {id} not found");
            }

            var customer = mapper.Map<Customer>(dto);
            customer.Id = id;
            store.Customers[index] = customer;

            return Task.FromResult(mapper.Map<CustomerDto>(customer));
        }
    }

    /// <inheritdoc/>
    public Task Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var customer = store.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Customer with Id {id} not found");

            store.Customers.Remove(customer);
            return Task.CompletedTask;
        }
    }
}
