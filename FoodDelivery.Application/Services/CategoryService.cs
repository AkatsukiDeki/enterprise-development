using FoodDelivery.Application.Contracts.Categories;
using FoodDelivery.Domain.Entities;
using FoodDelivery.Infrastructure.InMemory;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления категориями блюд.
/// </summary>
/// <param name="store">Хранилище данных в памяти.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class CategoryService(InMemoryFoodDeliveryStore store, IMapper mapper) : ICategoryService
{
    /// <inheritdoc/>
    public Task<IReadOnlyList<CategoryDto>> GetAll()
    {
        lock (store.SyncRoot)
        {
            IReadOnlyList<CategoryDto> result = store.Categories
                .Select(mapper.Map<CategoryDto>)
                .ToList();
            return Task.FromResult(result);
        }
    }

    /// <inheritdoc/>
    public Task<CategoryDto> Get(int id)
    {
        lock (store.SyncRoot)
        {
            var category = store.Categories.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Category with Id {id} not found");

            return Task.FromResult(mapper.Map<CategoryDto>(category));
        }
    }

    /// <inheritdoc/>
    public Task<CategoryDto> Create(CategoryCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var category = mapper.Map<Category>(dto);
            category.Id = store.NextCategoryId();

            store.Categories.Add(category);
            return Task.FromResult(mapper.Map<CategoryDto>(category));
        }
    }

    /// <inheritdoc/>
    public Task<CategoryDto> Update(int id, CategoryCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var index = store.Categories.FindIndex(c => c.Id == id);
            if (index < 0)
            {
                throw new KeyNotFoundException($"Category with Id {id} not found");
            }

            var category = mapper.Map<Category>(dto);
            category.Id = id;
            store.Categories[index] = category;

            return Task.FromResult(mapper.Map<CategoryDto>(category));
        }
    }

    /// <inheritdoc/>
    public Task Delete(int id)
    {
        lock (store.SyncRoot)
        {
            var category = store.Categories.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Category with Id {id} not found");

            if (store.Dishes.Any(d => d.Category.Id == id))
            {
                throw new InvalidOperationException($"Category with Id {id} is referenced by existing dishes and cannot be deleted.");
            }

            store.Categories.Remove(category);
            return Task.CompletedTask;
        }
    }
}
