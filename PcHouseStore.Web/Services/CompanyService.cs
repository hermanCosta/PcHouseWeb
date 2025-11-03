using PcHouseStore.Web.Models;

namespace PcHouseStore.Web.Services;

public class CompanyService
{
    private readonly ApiService _apiService;

    public CompanyService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<CompanyResponse>?> GetCompaniesAsync()
    {
        return await _apiService.GetListAsync<CompanyResponse>("api/companies");
    }

    public async Task<CompanyResponse?> GetCompanyAsync(long id)
    {
        return await _apiService.GetAsync<CompanyResponse>($"api/companies/{id}");
    }

    public async Task<CompanyResponse?> CreateCompanyAsync(CreateCompanyRequest request)
    {
        return await _apiService.PostAsync<CreateCompanyRequest, CompanyResponse>("api/companies", request);
    }

    public async Task<CompanyResponse?> UpdateCompanyAsync(long id, UpdateCompanyRequest request)
    {
        return await _apiService.PutAsync<UpdateCompanyRequest, CompanyResponse>($"api/companies/{id}", request);
    }

    public async Task<bool> DeleteCompanyAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/companies/{id}");
    }
}
