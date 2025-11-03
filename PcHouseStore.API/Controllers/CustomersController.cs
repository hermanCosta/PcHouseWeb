using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Person> _personRepository;
    private readonly PcHouseStoreDbContext _context;

    public CustomersController(
        IRepository<Customer> customerRepository,
        IRepository<Person> personRepository,
        PcHouseStoreDbContext context)
    {
        _customerRepository = customerRepository;
        _personRepository = personRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomers([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var customers = await _context.Customers
            .Include(c => c.Person)
            .Where(c => c.CompanyId == companyId)
            .ToListAsync();

        return Ok(customers.Select(c => CustomerMapper.ToResponse(c)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(long id)
    {
        var customer = await _context.Customers
            .Include(c => c.Person)
            .FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
            return NotFound();

        return Ok(CustomerMapper.ToResponse(customer));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> SearchCustomers([FromQuery] long companyId, [FromQuery] string searchTerm)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var customers = await _context.Customers
            .Include(c => c.Person)
            .Where(c => c.CompanyId == companyId &&
                       (c.Person.FirstName.Contains(searchTerm) ||
                        c.Person.LastName.Contains(searchTerm) ||
                        (c.Person.Email != null && c.Person.Email.Contains(searchTerm))))
            .ToListAsync();

        return Ok(customers.Select(c => CustomerMapper.ToResponse(c)));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var customer = new Customer
            {
                PersonId = request.PersonId,
                CompanyId = request.CompanyId,
                MarketingOptIn = request.MarketingOptIn,
                CreatedAt = DateTime.UtcNow
            };

            var createdCustomer = await _customerRepository.AddAsync(customer);
            
            var customerWithPerson = await _context.Customers
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.CustomerId == createdCustomer.CustomerId);

            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, 
                CustomerMapper.ToResponse(customerWithPerson!));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the customer: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerResponse>> UpdateCustomer(long id, [FromBody] UpdateCustomerRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            if (request.CompanyId.HasValue)
                customer.CompanyId = request.CompanyId;
            customer.MarketingOptIn = request.MarketingOptIn;

            await _customerRepository.UpdateAsync(customer);

            var updatedCustomer = await _context.Customers
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            return Ok(CustomerMapper.ToResponse(updatedCustomer!));
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
