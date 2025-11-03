using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;
using PcHouseStore.Infrastructure.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly IRepository<Company> _companyRepository;
    private readonly PcHouseStoreDbContext _context;

    public CompaniesController(IRepository<Company> companyRepository, PcHouseStoreDbContext context)
    {
        _companyRepository = companyRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyResponse>>> GetCompanies()
    {
        var companies = await _context.Companies
            .Include(c => c.BillingAddress)
            .Include(c => c.ShippingAddress)
            .ToListAsync();
        
        return Ok(companies.Select(c => CompanyMapper.ToResponse(c)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyResponse>> GetCompany(long id)
    {
        var company = await _context.Companies
            .Include(c => c.BillingAddress)
            .Include(c => c.ShippingAddress)
            .FirstOrDefaultAsync(c => c.CompanyId == id);
        
        if (company == null)
            return NotFound();

        return Ok(CompanyMapper.ToResponse(company));
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponse>> CreateCompany([FromBody] CreateCompanyRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var company = new Company
            {
                LegalName = request.LegalName,
                TradingName = request.TradingName,
                VatNumber = request.VatNumber,
                RegistrationNumber = request.RegistrationNumber,
                Email = request.Email,
                PhonePrimary = request.PhonePrimary,
                PhoneSecondary = request.PhoneSecondary,
                Website = request.Website,
                BillingAddressId = request.BillingAddressId,
                ShippingAddressId = request.ShippingAddressId,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            var createdCompany = await _companyRepository.AddAsync(company);
            
            // Reload with addresses for response
            var companyWithAddresses = await _context.Companies
                .Include(c => c.BillingAddress)
                .Include(c => c.ShippingAddress)
                .FirstOrDefaultAsync(c => c.CompanyId == createdCompany.CompanyId);
            
            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.CompanyId }, 
                CompanyMapper.ToResponse(companyWithAddresses!));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating the company: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CompanyResponse>> UpdateCompany(long id, [FromBody] UpdateCompanyRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                return NotFound();

            company.LegalName = request.LegalName;
            company.TradingName = request.TradingName;
            company.VatNumber = request.VatNumber;
            company.RegistrationNumber = request.RegistrationNumber;
            company.Email = request.Email;
            company.PhonePrimary = request.PhonePrimary;
            company.PhoneSecondary = request.PhoneSecondary;
            company.Website = request.Website;
            company.BillingAddressId = request.BillingAddressId;
            company.ShippingAddressId = request.ShippingAddressId;

            await _companyRepository.UpdateAsync(company);
            
            // Reload with addresses for response
            var updatedCompany = await _context.Companies
                .Include(c => c.BillingAddress)
                .Include(c => c.ShippingAddress)
                .FirstOrDefaultAsync(c => c.CompanyId == id);
            
            return Ok(CompanyMapper.ToResponse(updatedCompany!));
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

    private static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
