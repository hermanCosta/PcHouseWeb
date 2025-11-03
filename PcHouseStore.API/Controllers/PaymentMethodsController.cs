using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.API.Models;
using PcHouseStore.Infrastructure.Data;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodsController : ControllerBase
{
    private readonly PcHouseStoreDbContext _context;

    public PaymentMethodsController(PcHouseStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentMethodResponse>>> GetPaymentMethods([FromQuery] long companyId)
    {
        if (companyId <= 0)
            return BadRequest("Company ID is required");

        var paymentMethods = await _context.PaymentMethods
            .Where(pm => pm.CompanyId == companyId && pm.IsActive)
            .ToListAsync();
        return Ok(paymentMethods.Select(pm => PaymentMethodMapper.ToResponse(pm)));
    }
}

