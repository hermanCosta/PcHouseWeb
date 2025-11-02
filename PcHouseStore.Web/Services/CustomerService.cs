using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class CustomerService
{
    private readonly ApiService _apiService;

    public CustomerService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<Customer>?> GetCustomersAsync(long companyId)
    {
        return await _apiService.GetListAsync<Customer>($"api/customers?companyId={companyId}");
    }

    public async Task<Customer?> GetCustomerAsync(long id)
    {
        return await _apiService.GetAsync<Customer>($"api/customers/{id}");
    }

    public async Task<IEnumerable<Customer>?> SearchCustomersAsync(long companyId, string searchTerm)
    {
        return await _apiService.GetListAsync<Customer>($"api/customers/search?companyId={companyId}&searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<Customer?> CreateCustomerAsync(Customer customer)
    {
        return await _apiService.PostAsync<Customer>("api/customers", customer);
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        return await _apiService.PutAsync<Customer>($"api/customers/{customer.CustomerId}", customer);
    }

    public async Task<bool> DeleteCustomerAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/customers/{id}");
    }
}
