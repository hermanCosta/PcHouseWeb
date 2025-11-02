using PcHouseStore.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace PcHouseStore.Web.Services;

public class AuthenticationService
{
    private readonly CompanyService _companyService;
    private Company? _currentCompany;

    public AuthenticationService(CompanyService companyService)
    {
        _companyService = companyService;
    }

    public Company? CurrentCompany => _currentCompany;
    public bool IsAuthenticated => _currentCompany != null;

    public async Task<bool> LoginAsync(string companyName, string password)
    {
        try
        {
            var companies = await _companyService.GetCompaniesAsync();
            if (companies == null) return false;

            var company = companies.FirstOrDefault(c => 
                c.Name.Equals(companyName, StringComparison.OrdinalIgnoreCase));

            if (company == null) return false;

            // For now, we'll do a simple password comparison
            // In a real application, you should hash passwords and compare hashes
            if (company.Password == password)
            {
                _currentCompany = company;
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

    public void Logout()
    {
        _currentCompany = null;
    }

    public string EncryptPassword(string password)
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
