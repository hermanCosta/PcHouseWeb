using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;

namespace PcHouseStore.Infrastructure.Repositories;

public class ProductServiceRepository : Repository<ProductService>, IProductServiceRepository
{
    public ProductServiceRepository(PcHouseStoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductService>> GetProductsByCompanyAsync(long companyId)
    {
        return await _dbSet
            .Where(ps => ps.CompanyId == companyId)
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductService>> SearchProductsAsync(string searchTerm)
    {
        searchTerm = searchTerm.Trim();
        return await _dbSet
            .Where(ps => ps.Name.Contains(searchTerm) ||
                        (ps.Category != null && ps.Category.Contains(searchTerm)) ||
                        (ps.Note != null && ps.Note.Contains(searchTerm)))
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductService>> GetLowStockProductsAsync(long companyId)
    {
        return await _dbSet
            .Where(ps => ps.CompanyId == companyId && ps.Quantity <= ps.MinQuantity)
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductService>> GetProductsByCategoryAsync(long companyId, string category)
    {
        return await _dbSet
            .Where(ps => ps.CompanyId == companyId && ps.Category == category)
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<bool> CheckProductExistsAsync(string productName, long companyId)
    {
        return await _dbSet
            .AnyAsync(ps => ps.CompanyId == companyId && ps.Name == productName);
    }
}

