using Microsoft.AspNetCore.Mvc;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders([FromQuery] long companyId, [FromQuery] string? orderType)
    {
        if (companyId <= 0)
        {
            return BadRequest("Company ID is required");
        }

        if (!TryParseOrderType(orderType, out var parsedType, allowNull: true))
        {
            return BadRequest("Invalid order type");
        }

        var orders = await _orderRepository.GetOrdersByCompanyAsync(companyId, parsedType);
        return Ok(orders.Select(o => OrderMapper.ToResponse(o)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(long id)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(OrderMapper.ToResponse(order, includeDetails: true));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> SearchOrders([FromQuery] long companyId, [FromQuery] string searchTerm, [FromQuery] string? orderType)
    {
        if (companyId <= 0)
        {
            return BadRequest("Company ID is required");
        }

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term is required");
        }

        if (!TryParseOrderType(orderType, out var parsedType, allowNull: true))
        {
            return BadRequest("Invalid order type");
        }

        var orders = await _orderRepository.SearchOrdersAsync(companyId, searchTerm, parsedType);
        return Ok(orders.Select(o => OrderMapper.ToResponse(o)));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByCustomer(long customerId, [FromQuery] string? orderType)
    {
        if (customerId <= 0)
        {
            return BadRequest("Customer ID is required");
        }

        if (!TryParseOrderType(orderType, out var parsedType, allowNull: true))
        {
            return BadRequest("Invalid order type");
        }

        var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId, parsedType);
        return Ok(orders.Select(o => OrderMapper.ToResponse(o)));
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByDateRange(
        [FromQuery] long companyId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string? orderType)
    {
        if (companyId <= 0)
        {
            return BadRequest("Company ID is required");
        }

        if (startDate > endDate)
        {
            return BadRequest("Start date cannot be greater than end date");
        }

        if (!TryParseOrderType(orderType, out var parsedType, allowNull: true))
        {
            return BadRequest("Invalid order type");
        }

        var orders = await _orderRepository.GetOrdersByDateRangeAsync(companyId, startDate, endDate, parsedType);
        return Ok(orders.Select(o => OrderMapper.ToResponse(o)));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Order order;
        try
        {
            order = OrderFactory.CreateOrder(request);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }

        try
        {
            var created = await _orderRepository.AddAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = created.OrderId }, OrderMapper.ToResponse(created, includeDetails: true));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the order: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(long id, [FromBody] UpdateOrderRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = await _orderRepository.GetOrderWithDetailsAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        if (request.Status.HasValue)
        {
            existing.Status = request.Status.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            existing.Notes = request.Notes;
        }

        if (request.BalanceDue.HasValue)
        {
            existing.BalanceDue = request.BalanceDue.Value;
        }

        existing.ClosedAt = request.ClosedAt ?? existing.ClosedAt;

        try
        {
            await _orderRepository.UpdateAsync(existing);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the order: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(long id)
    {
        try
        {
            await _orderRepository.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the order: {ex.Message}");
        }
    }

    private static bool TryParseOrderType(string? orderType, out OrderType? parsedType, bool allowNull = false)
    {
        parsedType = null;

        if (string.IsNullOrWhiteSpace(orderType))
        {
            return allowNull;
        }

        if (Enum.TryParse<OrderType>(orderType, true, out var parsed))
        {
            parsedType = parsed;
            return true;
        }

        return false;
    }

}
