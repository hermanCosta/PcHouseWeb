using Microsoft.AspNetCore.Mvc;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Person> _personRepository;

    public CustomersController(IRepository<Customer> customerRepository, IRepository<Person> personRepository)
    {
        _customerRepository = customerRepository;
        _personRepository = personRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var customers = await _customerRepository.FindAsync(c => c.CompanyId == companyId);
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(long id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers([FromQuery] long companyId, [FromQuery] string searchTerm)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        // This would need to be implemented with proper joins in a more complex scenario
        var customers = await _customerRepository.FindAsync(c => c.CompanyId == companyId);
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdCustomer = await _customerRepository.AddAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CompanyId }, createdCustomer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the customer: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(long id, [FromBody] Customer customer)
    {
        if (id != customer.CustomerId)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _customerRepository.UpdateAsync(customer);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the customer: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(long id)
    {
        try
        {
            await _customerRepository.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the customer: {ex.Message}");
        }
    }
}
