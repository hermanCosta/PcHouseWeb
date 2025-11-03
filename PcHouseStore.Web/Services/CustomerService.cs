using PcHouseStore.Web.Models;

namespace PcHouseStore.Web.Services;

public class CustomerService
{
    private readonly ApiService _apiService;

    public CustomerService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<CustomerResponse>?> GetCustomersAsync(long companyId)
    {
        return await _apiService.GetListAsync<CustomerResponse>($"api/customers?companyId={companyId}");
    }

    public async Task<CustomerResponse?> GetCustomerAsync(long id)
    {
        return await _apiService.GetAsync<CustomerResponse>($"api/customers/{id}");
    }

    public async Task<IEnumerable<CustomerResponse>?> SearchCustomersAsync(long companyId, string searchTerm)
    {
        return await _apiService.GetListAsync<CustomerResponse>($"api/customers/search?companyId={companyId}&searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<CustomerResponse?> CreateCustomerAsync(CreateCustomerRequest request)
    {
        return await _apiService.PostAsync<CreateCustomerRequest, CustomerResponse>("api/customers", request);
    }

    public async Task<CustomerResponse?> UpdateCustomerAsync(long id, UpdateCustomerRequest request)
    {
        return await _apiService.PutAsync<UpdateCustomerRequest, CustomerResponse>($"api/customers/{id}", request);
    }

    public async Task<bool> DeleteCustomerAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/customers/{id}");
    }
}
