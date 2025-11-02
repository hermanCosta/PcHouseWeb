using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class SaleService
{
    private readonly ApiService _apiService;

    public SaleService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<Sale>?> GetSalesAsync(long companyId)
    {
        return await _apiService.GetListAsync<Sale>($"api/sales?companyId={companyId}");
    }

    public async Task<Sale?> GetSaleAsync(long id)
    {
        return await _apiService.GetAsync<Sale>($"api/sales/{id}");
    }

    public async Task<IEnumerable<Sale>?> SearchSalesAsync(long companyId, string searchTerm)
    {
        return await _apiService.GetListAsync<Sale>($"api/sales/search?companyId={companyId}&searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<IEnumerable<Sale>?> GetSalesByCustomerAsync(long customerId)
    {
        return await _apiService.GetListAsync<Sale>($"api/sales/customer/{customerId}");
    }

    public async Task<IEnumerable<Sale>?> GetSalesByEmployeeAsync(long employeeId)
    {
        return await _apiService.GetListAsync<Sale>($"api/sales/employee/{employeeId}");
    }

    public async Task<IEnumerable<Sale>?> GetSalesByDateRangeAsync(long companyId, DateTime startDate, DateTime endDate)
    {
        return await _apiService.GetListAsync<Sale>($"api/sales/date-range?companyId={companyId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
    }

    public async Task<Sale?> CreateSaleAsync(Sale sale)
    {
        return await _apiService.PostAsync<Sale>("api/sales", sale);
    }

    public async Task<bool> UpdateSaleAsync(Sale sale)
    {
        return await _apiService.PutAsync<Sale>($"api/sales/{sale.SaleId}", sale);
    }

    public async Task<bool> DeleteSaleAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/sales/{id}");
    }
}
