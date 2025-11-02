using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;

namespace PcHouseStore.Infrastructure.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(PcHouseStoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Sale>> GetSalesByCompanyAsync(long companyId)
    {
        return await _dbSet
            .Include(s => s.Customer)
                .ThenInclude(c => c.Person)
            .Include(s => s.Employee)
                .ThenInclude(e => e.Person)
            .Include(s => s.Company)
            .Include(s => s.Payments)
            .Include(s => s.SaleProductServices)
                .ThenInclude(sps => sps.ProductService)
            .Where(s => s.CompanyId == companyId)
            .OrderByDescending(s => s.Created)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> SearchSalesAsync(long companyId, string searchTerm)
    {
        return await _dbSet
            .Include(s => s.Customer)
                .ThenInclude(c => c.Person)
            .Include(s => s.Employee)
                .ThenInclude(e => e.Person)
            .Include(s => s.Company)
            .Where(s => s.CompanyId == companyId &&
                       (s.Customer.Person.FirstName.Contains(searchTerm) ||
                        s.Customer.Person.LastName.Contains(searchTerm) ||
                        s.Customer.Person.ContactNo.Contains(searchTerm) ||
                        s.ImportantNotes!.Contains(searchTerm)))
            .OrderByDescending(s => s.Created)
            .ToListAsync();
    }

    public async Task<Sale?> GetSaleWithDetailsAsync(long saleId)
    {
        return await _dbSet
            .Include(s => s.Customer)
                .ThenInclude(c => c.Person)
            .Include(s => s.Employee)
                .ThenInclude(e => e.Person)
            .Include(s => s.Company)
            .Include(s => s.Payments)
            .Include(s => s.SaleProductServices)
                .ThenInclude(sps => sps.ProductService)
            .FirstOrDefaultAsync(s => s.SaleId == saleId);
    }

    public async Task<IEnumerable<Sale>> GetSalesByCustomerAsync(long customerId)
    {
        return await _dbSet
            .Include(s => s.Customer)
                .ThenInclude(c => c.Person)
            .Include(s => s.Employee)
                .ThenInclude(e => e.Person)
            .Include(s => s.Payments)
            .Include(s => s.SaleProductServices)
                .ThenInclude(sps => sps.ProductService)
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.Created)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByEmployeeAsync(long employeeId)
    {
        return await _dbSet
            .Include(s => s.Customer)
                .ThenInclude(c => c.Person)
            .Include(s => s.Employee)
                .ThenInclude(e => e.Person)
            .Include(s => s.Payments)
            .Include(s => s.SaleProductServices)
                .ThenInclude(sps => sps.ProductService)
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.Created)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(long companyId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(s => s.Customer)
                .ThenInclude(c => c.Person)
            .Include(s => s.Employee)
                .ThenInclude(e => e.Person)
            .Include(s => s.Payments)
            .Include(s => s.SaleProductServices)
                .ThenInclude(sps => sps.ProductService)
            .Where(s => s.CompanyId == companyId &&
                       s.Created >= startDate &&
                       s.Created <= endDate)
            .OrderByDescending(s => s.Created)
            .ToListAsync();
    }
}
