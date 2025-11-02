using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class CompanyService
{
    private readonly ApiService _apiService;

    public CompanyService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<Company>?> GetCompaniesAsync()
    {
        return await _apiService.GetListAsync<Company>("api/companies");
    }

    public async Task<Company?> GetCompanyAsync(long id)
    {
        return await _apiService.GetAsync<Company>($"api/companies/{id}");
    }

    public async Task<Company?> CreateCompanyAsync(Company company)
    {
        return await _apiService.PostAsync<Company>("api/companies", company);
    }

    public async Task<bool> UpdateCompanyAsync(Company company)
    {
        return await _apiService.PutAsync<Company>($"api/companies/{company.CompanyId}", company);
    }

    public async Task<bool> DeleteCompanyAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/companies/{id}");
    }
}
