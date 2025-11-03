using PcHouseStore.Domain.Models;

namespace PcHouseStore.Infrastructure.Repositories;

public interface IProductServiceRepository : IRepository<ProductService>
{
    Task<IEnumerable<ProductService>> GetProductsByCompanyAsync(long companyId);
    Task<IEnumerable<ProductService>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<ProductService>> GetLowStockProductsAsync(long companyId);
    Task<IEnumerable<ProductService>> GetProductsByCategoryAsync(long companyId, string category);
    Task<bool> CheckProductExistsAsync(string productName, long companyId);
}

