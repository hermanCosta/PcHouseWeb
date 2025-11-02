using PcHouseStore.Domain.Models;

namespace PcHouseStore.Infrastructure.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<IEnumerable<Sale>> GetSalesByCompanyAsync(long companyId);
    Task<IEnumerable<Sale>> SearchSalesAsync(long companyId, string searchTerm);
    Task<Sale?> GetSaleWithDetailsAsync(long saleId);
    Task<IEnumerable<Sale>> GetSalesByCustomerAsync(long customerId);
    Task<IEnumerable<Sale>> GetSalesByEmployeeAsync(long employeeId);
    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(long companyId, DateTime startDate, DateTime endDate);
}
