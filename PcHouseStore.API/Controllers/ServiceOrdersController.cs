using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceOrdersController : ControllerBase
{
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly IRepository<OrderStatusEvent> _statusEventRepository;
    private readonly IRepository<OrderNote> _orderNoteRepository;
    private readonly PcHouseStoreDbContext _context;

    public ServiceOrdersController(
        IServiceOrderRepository serviceOrderRepository,
        IRepository<OrderStatusEvent> statusEventRepository,
        IRepository<OrderNote> orderNoteRepository,
        PcHouseStoreDbContext context)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _statusEventRepository = statusEventRepository;
        _orderNoteRepository = orderNoteRepository;
        _context = context;
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
            
            // Create initial status event
            var initialStatusEvent = new OrderStatusEvent
            {
                OrderId = createdServiceOrder.OrderId,
                Status = createdServiceOrder.Status,
                ChangedByEmployeeId = createdServiceOrder.CreatedByEmployeeId,
                ChangedAt = createdServiceOrder.PlacedAt,
                Comment = "Order created"
            };
            await _statusEventRepository.AddAsync(initialStatusEvent);
            
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

        var oldStatus = existing.Status;
        var statusComment = request.Notes; // Store for status event comment
        
        if (request.Status.HasValue && request.Status.Value != existing.Status)
        {
            existing.Status = request.Status.Value;
            
            // Create status event when status changes
            // Use Notes field as comment for status change (if provided)
            var statusEvent = new OrderStatusEvent
            {
                OrderId = existing.OrderId,
                Status = existing.Status,
                ChangedByEmployeeId = existing.CreatedByEmployeeId, // TODO: Get from auth context
                ChangedAt = DateTime.UtcNow,
                Comment = statusComment // Comments from Notes field go to status event
            };
            await _statusEventRepository.AddAsync(statusEvent);
            
            // Don't update Notes field when it's used for status comment
            // Notes field should be separate from status change comments
            statusComment = null; // Clear so Notes field doesn't get updated below
        }

        // Update Notes field only if not used for status change comment
        if (!string.IsNullOrWhiteSpace(statusComment))
        {
            existing.Notes = statusComment;
        }

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

    [HttpPost("{id}/notes")]
    public async Task<ActionResult<OrderNoteResponse>> AddNote(long id, [FromBody] AddOrderNoteRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = await _serviceOrderRepository.GetServiceOrderWithDetailsAsync(id);
        if (order == null)
            return NotFound();

        var orderNote = new OrderNote
        {
            OrderId = id,
            EmployeeId = order.CreatedByEmployeeId, // TODO: Get from auth context
            Note = request.Note,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            var createdNote = await _orderNoteRepository.AddAsync(orderNote);
            
            // Load the note with employee details for response
            var noteWithDetails = await _context.OrderNotes
                .Include(on => on.Employee)
                    .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(on => on.OrderNoteId == createdNote.OrderNoteId);

            if (noteWithDetails == null)
                return StatusCode(500, "Note was created but could not be retrieved");

            var response = new OrderNoteResponse(
                noteWithDetails.OrderNoteId,
                noteWithDetails.EmployeeId,
                noteWithDetails.Employee?.Person?.DisplayName,
                noteWithDetails.Note,
                noteWithDetails.CreatedAt
            );

            return CreatedAtAction(nameof(GetServiceOrder), new { id }, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while adding the note: {ex.Message}");
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
