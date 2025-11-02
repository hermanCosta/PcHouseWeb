using PcHouseStore.Domain.Models;

namespace PcHouseStore.Web.Services;

public class ProductService
{
    private readonly ApiService _apiService;

    public ProductService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IEnumerable<PcHouseStore.Domain.Models.ProductService>?> GetProductsAsync(long companyId)
    {
        return await _apiService.GetListAsync<PcHouseStore.Domain.Models.ProductService>($"api/productservices?companyId={companyId}");
    }

    public async Task<PcHouseStore.Domain.Models.ProductService?> GetProductAsync(long id)
    {
        return await _apiService.GetAsync<PcHouseStore.Domain.Models.ProductService>($"api/productservices/{id}");
    }

    public async Task<IEnumerable<PcHouseStore.Domain.Models.ProductService>?> SearchProductsAsync(string searchTerm)
    {
        return await _apiService.GetListAsync<PcHouseStore.Domain.Models.ProductService>($"api/productservices/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
    }

    public async Task<IEnumerable<PcHouseStore.Domain.Models.ProductService>?> GetLowStockProductsAsync(long companyId)
    {
        return await _apiService.GetListAsync<PcHouseStore.Domain.Models.ProductService>($"api/productservices/low-stock?companyId={companyId}");
    }

    public async Task<IEnumerable<PcHouseStore.Domain.Models.ProductService>?> GetProductsByCategoryAsync(long companyId, string category)
    {
        return await _apiService.GetListAsync<PcHouseStore.Domain.Models.ProductService>($"api/productservices/category/{Uri.EscapeDataString(category)}?companyId={companyId}");
    }

    public async Task<bool> CheckProductExistsAsync(string productName, long companyId)
    {
        var result = await _apiService.GetAsync<bool>($"api/productservices/check-exists?productName={Uri.EscapeDataString(productName)}&companyId={companyId}");
        return result;
    }

    public async Task<PcHouseStore.Domain.Models.ProductService?> CreateProductAsync(PcHouseStore.Domain.Models.ProductService product)
    {
        return await _apiService.PostAsync<PcHouseStore.Domain.Models.ProductService>("api/productservices", product);
    }

    public async Task<bool> UpdateProductAsync(PcHouseStore.Domain.Models.ProductService product)
    {
        return await _apiService.PutAsync<PcHouseStore.Domain.Models.ProductService>($"api/productservices/{product.ProductServiceId}", product);
    }

    public async Task<bool> DeleteProductAsync(long id)
    {
        return await _apiService.DeleteAsync($"api/productservices/{id}");
    }
}
