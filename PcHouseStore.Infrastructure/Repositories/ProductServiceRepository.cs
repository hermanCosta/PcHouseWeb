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
            .Include(ps => ps.Company)
            .Where(ps => ps.CompanyId == companyId)
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductService>> GetLowStockProductsAsync(long companyId)
    {
        return await _dbSet
            .Include(ps => ps.Company)
            .Where(ps => ps.CompanyId == companyId && ps.Quantity <= ps.MinQuantity)
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductService>> SearchProductsAsync(string searchTerm)
    {
        return await _dbSet
            .Include(ps => ps.Company)
            .Where(ps => ps.Name.Contains(searchTerm) ||
                        ps.Category!.Contains(searchTerm) ||
                        ps.Note!.Contains(searchTerm))
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductService>> GetProductsByCategoryAsync(long companyId, string category)
    {
        return await _dbSet
            .Include(ps => ps.Company)
            .Where(ps => ps.CompanyId == companyId && ps.Category == category)
            .OrderBy(ps => ps.Name)
            .ToListAsync();
    }

    public async Task<bool> CheckProductExistsAsync(string productName, long companyId)
    {
        return await _dbSet
            .AnyAsync(ps => ps.Name == productName && ps.CompanyId == companyId);
    }

    public async Task<ProductService?> GetProductByNameAsync(string productName, long companyId)
    {
        return await _dbSet
            .Include(ps => ps.Company)
            .FirstOrDefaultAsync(ps => ps.Name == productName && ps.CompanyId == companyId);
    }
}
