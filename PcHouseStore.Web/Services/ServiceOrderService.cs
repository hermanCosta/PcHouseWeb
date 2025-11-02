using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class ServiceOrderService
{
    private readonly ApiService _apiService;

    public ServiceOrderService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<ServiceOrder>?> GetServiceOrdersAsync(long companyId)
    {
        return await _apiService.GetListAsync<ServiceOrder>($"api/serviceorders?companyId={companyId}");
    }

    public async Task<ServiceOrder?> GetServiceOrderAsync(long id)
    {
        return await _apiService.GetAsync<ServiceOrder>($"api/serviceorders/{id}");
    }

    public async Task<IEnumerable<ServiceOrder>?> SearchServiceOrdersAsync(long companyId, string searchTerm)
    {
        return await _apiService.GetListAsync<ServiceOrder>($"api/serviceorders/search?companyId={companyId}&searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<IEnumerable<ServiceOrder>?> GetServiceOrdersByCustomerAsync(long customerId)
    {
        return await _apiService.GetListAsync<ServiceOrder>($"api/serviceorders/customer/{customerId}");
    }

    public async Task<IEnumerable<ServiceOrder>?> GetServiceOrdersByEmployeeAsync(long employeeId)
    {
        return await _apiService.GetListAsync<ServiceOrder>($"api/serviceorders/employee/{employeeId}");
    }

    public async Task<IEnumerable<ServiceOrder>?> GetServiceOrdersByStatusAsync(long companyId, Domain.Enums.OrderStatus status)
    {
        return await _apiService.GetListAsync<ServiceOrder>($"api/serviceorders/status/{status}?companyId={companyId}");
    }

    public async Task<long> GetLastOrderIdAsync()
    {
        var result = await _apiService.GetAsync<long>("api/serviceorders/last-order-id");
        return result;
    }

    public async Task<ServiceOrder?> CreateServiceOrderAsync(ServiceOrder serviceOrder)
    {
        return await _apiService.PostAsync<ServiceOrder>("api/serviceorders", serviceOrder);
    }

    public async Task<bool> UpdateServiceOrderAsync(ServiceOrder serviceOrder)
    {
        return await _apiService.PutAsync<ServiceOrder>($"api/serviceorders/{serviceOrder.ServiceOrderId}", serviceOrder);
    }

    public async Task<bool> DeleteServiceOrderAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/serviceorders/{id}");
    }
}
