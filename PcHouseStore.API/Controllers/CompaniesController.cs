using Microsoft.AspNetCore.Mvc;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Repositories;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly IRepository<Company> _companyRepository;

    public CompaniesController(IRepository<Company> companyRepository)
    {
        _companyRepository = companyRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
    {
        var companies = await _companyRepository.GetAllAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetCompany(long id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
            return NotFound();

        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<Company>> CreateCompany([FromBody] Company company)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createdCompany = await _companyRepository.AddAsync(company);
            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.CompanyId }, createdCompany);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the company: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCompany(long id, [FromBody] Company company)
    {
        if (id != company.CompanyId)
            return BadRequest("ID mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _companyRepository.UpdateAsync(company);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the company: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(long id)
    {
        try
        {
            await _companyRepository.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the company: {ex.Message}");
        }
    }
}
