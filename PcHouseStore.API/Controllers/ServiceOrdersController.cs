using Microsoft.AspNetCore.Mvc;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Enums;
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
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetServiceOrders([FromQuery] long companyId, [FromQuery] OrderStatus? status)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByCompanyAsync(companyId, status);
        return Ok(serviceOrders.Select(so => OrderMapper.ToResponse(so)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetServiceOrder(long id)
    {
        var serviceOrder = await _serviceOrderRepository.GetServiceOrderWithDetailsAsync(id);
        if (serviceOrder == null)
            return NotFound();

        return Ok(OrderMapper.ToResponse(serviceOrder, includeDetails: true));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> SearchServiceOrders([FromQuery] long companyId, [FromQuery] string searchTerm)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var serviceOrders = await _serviceOrderRepository.SearchServiceOrdersAsync(companyId, searchTerm);
        return Ok(serviceOrders.Select(so => OrderMapper.ToResponse(so)));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetServiceOrdersByCustomer(long customerId)
    {
        if (customerId <= 0)
            return BadRequest("Customer ID is required");

        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByCustomerAsync(customerId);
        return Ok(serviceOrders.Select(so => OrderMapper.ToResponse(so)));
    }

    [HttpGet("technician/{technicianId}")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetServiceOrdersByTechnician(long technicianId)
    {
        if (technicianId <= 0)
            return BadRequest("Technician ID is required");

        var serviceOrders = await _serviceOrderRepository.GetServiceOrdersByTechnicianAsync(technicianId);
        return Ok(serviceOrders.Select(so => OrderMapper.ToResponse(so)));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateServiceOrder([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!string.Equals(request.OrderType, OrderType.Service.ToString(), StringComparison.OrdinalIgnoreCase))
            return BadRequest("OrderType must be 'Service' for service orders.");

        Order order;
        try
        {
            order = OrderFactory.CreateOrder(request);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        if (order is not ServiceOrder serviceOrder)
            return BadRequest("Invalid payload for service order creation.");

        try
        {
            var createdServiceOrder = await _serviceOrderRepository.AddAsync(serviceOrder);
            return CreatedAtAction(nameof(GetServiceOrder), new { id = createdServiceOrder.OrderId }, OrderMapper.ToResponse(createdServiceOrder, includeDetails: true));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the service order: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateServiceOrder(long id, [FromBody] UpdateOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _serviceOrderRepository.GetServiceOrderWithDetailsAsync(id);
        if (existing is null)
            return NotFound();

        if (request.Status.HasValue)
            existing.Status = request.Status.Value;

        if (!string.IsNullOrWhiteSpace(request.Notes))
            existing.Notes = request.Notes;

        if (request.BalanceDue.HasValue)
            existing.BalanceDue = request.BalanceDue.Value;

        existing.ClosedAt = request.ClosedAt ?? existing.ClosedAt;

        try
        {
            await _serviceOrderRepository.UpdateAsync(existing);
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
