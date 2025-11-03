using Microsoft.AspNetCore.Mvc;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FaultsController : ControllerBase
{
    private readonly IRepository<Fault> _faultRepository;

    public FaultsController(IRepository<Fault> faultRepository)
    {
        _faultRepository = faultRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FaultResponse>>> GetFaults()
    {
        var faults = await _faultRepository.GetAllAsync();
        return Ok(faults.Select(f => FaultMapper.ToResponse(f)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FaultResponse>> GetFault(long id)
    {
        var fault = await _faultRepository.GetByIdAsync(id);
        if (fault == null)
            return NotFound();

        return Ok(FaultMapper.ToResponse(fault));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FaultResponse>>> SearchFaults([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest("Search term is required");

        var faults = await _faultRepository.FindAsync(f => 
            f.Description.Contains(searchTerm) || 
            (f.Code != null && f.Code.Contains(searchTerm)));
        return Ok(faults.Select(f => FaultMapper.ToResponse(f)));
    }

    [HttpPost]
    public async Task<ActionResult<FaultResponse>> CreateFault([FromBody] CreateFaultRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var fault = new Fault
            {
                Code = request.Code,
                Description = request.Description
            };

            var createdFault = await _faultRepository.AddAsync(fault);
            return CreatedAtAction(nameof(GetFault), new { id = createdFault.FaultId }, 
                FaultMapper.ToResponse(createdFault));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the fault: {ex.Message}");
        }
    }
}

