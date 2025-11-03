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
            var loginRequest = new { TradingName = companyName, Password = password };
            var company = await _companyService.LoginAsync(companyName, password);
            
            if (company == null) return false;

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
