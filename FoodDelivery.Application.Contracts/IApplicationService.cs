namespace FoodDelivery.Application.Contracts;

/// <summary>
/// Базовый контракт CRUD-сервиса приложения.
/// </summary>
/// <typeparam name="TDto">Тип DTO сущности.</typeparam>
/// <typeparam name="TCreateUpdateDto">Тип DTO для создания и обновления сущности.</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности.</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Создает новую сущность.
    /// </summary>
    /// <param name="dto">Данные для создания сущности.</param>
    /// <returns>Созданное DTO сущности.</returns>
    public TDto Create(TCreateUpdateDto dto);

    /// <summary>
    /// Возвращает сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Найденное DTO сущности.</returns>
    public TDto Get(TKey id);

    /// <summary>
    /// Возвращает все сущности.
    /// </summary>
    /// <returns>Список DTO всех сущностей.</returns>
    public IReadOnlyList<TDto> GetAll();

    /// <summary>
    /// Обновляет существующую сущность и возвращает её обновлённое представление.
    /// </summary>
    /// <param name="id">Идентификатор обновляемой сущности.</param>
    /// <param name="dto">Новые данные сущности.</param>
    /// <returns>Обновлённое DTO сущности.</returns>
    public TDto Update(TKey id, TCreateUpdateDto dto);

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности.</param>
    public void Delete(TKey id);
}
