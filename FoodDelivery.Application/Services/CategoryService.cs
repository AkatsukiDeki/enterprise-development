using FoodDelivery.Application.Contracts.Categories;
using FoodDelivery.Domain.Entities;
using MapsterMapper;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления категориями блюд.
/// </summary>
/// <param name="store">Хранилище данных.</param>
/// <param name="mapper">Маппер сущностей и DTO.</param>
public sealed class CategoryService(IFoodDeliveryStore store, IMapper mapper) : ICategoryService
{
    /// <inheritdoc/>
    public IReadOnlyList<CategoryDto> GetAll()
    {
        lock (store.SyncRoot)
        {
            return store.Categories
                .Select(mapper.Map<CategoryDto>)
                .ToList();
        }
    }

    /// <inheritdoc/>
    public CategoryDto Get(int id)
    {
        lock (store.SyncRoot)
        {
            var category = store.Categories.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Category with Id {id} not found");

            return mapper.Map<CategoryDto>(category);
        }
    }

    /// <inheritdoc/>
    public CategoryDto Create(CategoryCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var category = mapper.Map<Category>(dto);
            category.Id = store.NextCategoryId();

            store.Categories.Add(category);
            return mapper.Map<CategoryDto>(category);
        }
    }

    /// <inheritdoc/>
    public CategoryDto Update(int id, CategoryCreateUpdateDto dto)
    {
        lock (store.SyncRoot)
        {
            var category = store.Categories.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Category with Id {id} not found");

            mapper.Map(dto, category);
            return mapper.Map<CategoryDto>(category);
        }
    }

    /// <inheritdoc/>
    public void Delete(int id)
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
        }
    }
}
