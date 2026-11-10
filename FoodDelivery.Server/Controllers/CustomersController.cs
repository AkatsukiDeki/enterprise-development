using FoodDelivery.Domain.Entities;
using FoodDelivery.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly FoodDeliveryDbContext _db;

    public CustomersController(FoodDeliveryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll(CancellationToken ct)
    {
        return await _db.Customers.AsNoTracking().ToListAsync(ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> GetById(int id, CancellationToken ct)
    {
        var customer = await _db.Customers.FindAsync([id], ct);
        return customer is null ? NotFound() : customer;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create([FromBody] Customer customer, CancellationToken ct)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Customer updated, CancellationToken ct)
    {
        var customer = await _db.Customers.FindAsync([id], ct);
        if (customer is null) return NotFound();

        customer.LastName = updated.LastName;
        customer.FirstName = updated.FirstName;
        customer.Patronymic = updated.Patronymic;
        customer.PhoneNumber = updated.PhoneNumber;
        customer.DeliveryAddress = updated.DeliveryAddress;

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var customer = await _db.Customers.FindAsync([id], ct);
        if (customer is null) return NotFound();

        _db.Customers.Remove(customer);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}