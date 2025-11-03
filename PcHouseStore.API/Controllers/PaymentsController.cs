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
public class PaymentsController : ControllerBase
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly PcHouseStoreDbContext _context;

    public PaymentsController(IRepository<Payment> paymentRepository, PcHouseStoreDbContext context)
    {
        _paymentRepository = paymentRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDetailResponse>>> GetPayments([FromQuery] long? orderId)
    {
        IQueryable<Payment> query = _context.Payments
            .Include(p => p.PaymentMethod)
            .Include(p => p.Employee)
            .ThenInclude(e => e.Person);

        if (orderId.HasValue)
        {
            query = query.Where(p => p.OrderId == orderId);
        }

        var payments = await query.ToListAsync();
        return Ok(payments.Select(p => PaymentMapper.ToResponse(p)));
    }

    [HttpGet("deposits")]
    public async Task<ActionResult<IEnumerable<PaymentDetailResponse>>> GetDeposits([FromQuery] long orderId)
    {
        var deposits = await _context.Payments
            .Include(p => p.PaymentMethod)
            .Include(p => p.Employee)
            .ThenInclude(e => e.Person)
            .Where(p => p.OrderId == orderId && p.PaymentType == PaymentType.Deposit)
            .ToListAsync();
        return Ok(deposits.Select(p => PaymentMapper.ToResponse(p)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDetailResponse>> GetPayment(long id)
    {
        var payment = await _context.Payments
            .Include(p => p.PaymentMethod)
            .Include(p => p.Employee)
            .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(p => p.PaymentId == id);

        if (payment == null)
            return NotFound();

        return Ok(PaymentMapper.ToResponse(payment));
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDetailResponse>> CreatePayment([FromBody] CreatePaymentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var payment = new Payment
            {
                CompanyId = request.CompanyId,
                OrderId = request.OrderId,
                PaymentMethodId = request.PaymentMethodId,
                PaymentType = request.PaymentType,
                Amount = request.Amount,
                NetCash = request.NetCash,
                NetCard = request.NetCard,
                NetVoucher = request.NetVoucher,
                EmployeeId = request.EmployeeId,
                Reference = request.Reference,
                Notes = request.Notes,
                ProcessedAt = DateTime.UtcNow
            };

            var createdPayment = await _paymentRepository.AddAsync(payment);
            
            var paymentWithDetails = await _context.Payments
                .Include(p => p.PaymentMethod)
                .Include(p => p.Employee)
                .ThenInclude(e => e.Person)
                .FirstOrDefaultAsync(p => p.PaymentId == createdPayment.PaymentId);

            return CreatedAtAction(nameof(GetPayment), new { id = createdPayment.PaymentId }, 
                PaymentMapper.ToResponse(paymentWithDetails!));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the payment: {ex.Message}");
        }
    }
}

