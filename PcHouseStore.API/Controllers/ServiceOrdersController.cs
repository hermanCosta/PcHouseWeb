using Microsoft.AspNetCore.Mvc;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;

    public ServiceOrdersController(IServiceOrderRepository serviceOrderRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetServiceOrders([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByCompanyAsync(companyId);
        return Ok(serviceOrders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceOrder>> GetServiceOrder(long id)
    {
        var serviceOrder = await _serviceOrderRepository.GetServiceOrderWithDetailsAsync(id);
        if (serviceOrder == null)
            return NotFound();

        return Ok(serviceOrder);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ServiceOrder>>> SearchServiceOrders([FromQuery] long companyId, [FromQuery] string searchTerm)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var serviceOrders = await _serviceOrderRepository.SearchServiceOrdersAsync(companyId, searchTerm);
        return Ok(serviceOrders);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetServiceOrdersByCustomer(long customerId)
    {
        if (customerId <= 0)
            return BadRequest("Customer ID is required");

        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByCustomerAsync(customerId);
        return Ok(serviceOrders);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetServiceOrdersByEmployee(long employeeId)
    {
        if (employeeId <= 0)
            return BadRequest("Employee ID is required");

        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByEmployeeAsync(employeeId);
        return Ok(serviceOrders);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<ServiceOrder>>> GetServiceOrdersByStatus([FromQuery] long companyId, [FromRoute] Domain.Enums.OrderStatus status)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByStatusAsync(companyId, status);
        return Ok(serviceOrders);
    }

    [HttpGet("last-order-id")]
    public async Task<ActionResult<long>> GetLastOrderId()
    {
        var lastOrderId = await _serviceOrderRepository.GetLastOrderIdAsync();
        return Ok(lastOrderId);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceOrder>> CreateServiceOrder([FromBody] ServiceOrder serviceOrder)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdServiceOrder = await _serviceOrderRepository.AddAsync(serviceOrder);
            return CreatedAtAction(nameof(GetServiceOrder), new { id = createdServiceOrder.ServiceOrderId }, createdServiceOrder);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the service order: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateServiceOrder(long id, [FromBody] ServiceOrder serviceOrder)
    {
        if (id != serviceOrder.ServiceOrderId)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _serviceOrderRepository.UpdateAsync(serviceOrder);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the service order: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceOrder(long id)
    {
        try
        {
            await _serviceOrderRepository.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the service order: {ex.Message}");
        }
    }
}
