using FoodDelivery.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers;

/// <summary>
/// Базовый тонкий CRUD-контроллер.
/// </summary>
/// <typeparam name="TDto">Тип DTO сущности.</typeparam>
/// <typeparam name="TCreateUpdateDto">Тип DTO для создания и обновления.</typeparam>
/// <typeparam name="TService">Тип прикладного сервиса.</typeparam>
/// <param name="service">Прикладной CRUD-сервис.</param>
[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TDto, TCreateUpdateDto, TService>(TService service) : ControllerBase
    where TDto : class, IHasId<int>
    where TCreateUpdateDto : class
    where TService : IApplicationService<TDto, TCreateUpdateDto, int>
{
    /// <summary>
    /// Прикладной сервис сущности.
    /// </summary>
    protected TService Service { get; } = service;

    /// <summary>
    /// Возвращает все сущности.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TDto>>> GetAll()
    {
        var items = await Service.GetAll();
        return Ok(items);
    }

    /// <summary>
    /// Возвращает сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TDto>> GetById(int id)
    {
        var item = await Service.Get(id);
        return Ok(item);
    }

    /// <summary>
    /// Создает новую сущность.
    /// </summary>
    /// <param name="dto">Данные для создания сущности.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TDto>> Create([FromBody] TCreateUpdateDto dto)
    {
        var created = await Service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновляет существующую сущность.
    /// </summary>
    /// <param name="id">Идентификатор обновляемой сущности.</param>
    /// <param name="dto">Новые данные сущности.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TDto>> Update(int id, [FromBody] TCreateUpdateDto dto)
    {
        var updated = await Service.Update(id, dto);
        return Ok(updated);
    }

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой сущности.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        await Service.Delete(id);
        return NoContent();
    }
}
