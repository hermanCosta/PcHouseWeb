using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.API.Models;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace PcHouseStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly PcHouseStoreDbContext _context;

    public AuthController(PcHouseStoreDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<CompanyResponse>> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TradingName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Trading name and password are required." });
        }

        // Use EF.Functions.Like or ToLower() for case-insensitive comparison that can be translated to SQL
        var tradingNameLower = request.TradingName.ToLower();
        var company = await _context.Companies
            .Include(c => c.BillingAddress)
            .Include(c => c.ShippingAddress)
            .FirstOrDefaultAsync(c => c.TradingName != null && 
                c.TradingName.ToLower() == tradingNameLower);

        if (company == null)
        {
            return Unauthorized(new { message = "Invalid trading name or password." });
        }

        // Verify password
        var providedHash = HashPassword(request.Password);
        if (company.PasswordHash != providedHash)
        {
            return Unauthorized(new { message = "Invalid trading name or password." });
        }

        return Ok(CompanyMapper.ToResponse(company));
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

public record LoginRequest(string TradingName, string Password);

