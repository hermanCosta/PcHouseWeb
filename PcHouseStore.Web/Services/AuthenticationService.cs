using PcHouseStore.Web.Models;
using System.Security.Cryptography;
using System.Text;

namespace PcHouseStore.Web.Services;

public class AuthenticationService
{
    private readonly CompanyService _companyService;
    private CompanyResponse? _currentCompany;

    public AuthenticationService(CompanyService companyService)
    {
        _companyService = companyService;
    }

    public CompanyResponse? CurrentCompany => _currentCompany;
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

            // TODO: Implement proper authentication via API endpoint
            // For now, password validation should be done on the API side
            // This is a temporary solution - in production, create a login API endpoint
            // that validates credentials and returns authentication tokens
            // Since CompanyResponse doesn't contain password, we'll accept any match by name
            // This should be replaced with proper API-based authentication
            _currentCompany = company;
            return true;
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
