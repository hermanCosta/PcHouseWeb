using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CashMovementsController : ControllerBase
{
    private readonly IRepository<CashMovement> _cashMovementRepository;
    private readonly PcHouseStoreDbContext _context;

    public CashMovementsController(IRepository<CashMovement> cashMovementRepository, PcHouseStoreDbContext context)
    {
        _cashMovementRepository = cashMovementRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CashMovementResponse>>> GetCashMovements([FromQuery] long? paymentId)
    {
        IQueryable<CashMovement> query = _context.CashMovements
            .Include(cm => cm.Employee)
            .ThenInclude(e => e.Person);

        if (paymentId.HasValue)
        {
            query = query.Where(cm => cm.RelatedPaymentId == paymentId);
        }

        var cashMovements = await query.OrderByDescending(cm => cm.OccurredAt).ToListAsync();
        return Ok(cashMovements.Select(cm => CashMovementMapper.ToResponse(cm)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CashMovementResponse>> GetCashMovement(long id)
    {
        var cashMovement = await _context.CashMovements
            .Include(cm => cm.Employee)
            .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(cm => cm.CashMovementId == id);

        if (cashMovement == null)
            return NotFound();

        return Ok(CashMovementMapper.ToResponse(cashMovement));
    }

    [HttpPost]
    public async Task<ActionResult<CashMovementResponse>> CreateCashMovement([FromBody] CreateCashMovementRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var cashMovement = new CashMovement
            {
                CompanyId = request.CompanyId,
                EmployeeId = request.EmployeeId,
                MovementType = request.MovementType,
                Amount = request.Amount,
                Reason = request.Reason,
                RelatedPaymentId = request.RelatedPaymentId,
                OccurredAt = DateTime.UtcNow
            };

            var createdCashMovement = await _cashMovementRepository.AddAsync(cashMovement);
            
            var cashMovementWithDetails = await _context.CashMovements
                .Include(cm => cm.Employee)
                .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(cm => cm.CashMovementId == createdCashMovement.CashMovementId);

            return CreatedAtAction(nameof(GetCashMovement), new { id = createdCashMovement.CashMovementId }, 
                CashMovementMapper.ToResponse(cashMovementWithDetails!));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the cash movement: {ex.Message}");
        }
    }
}

