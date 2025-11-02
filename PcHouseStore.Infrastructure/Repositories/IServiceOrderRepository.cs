using PcHouseStore.Domain.Models;

namespace PcHouseStore.Infrastructure.Repositories;

public interface IServiceOrderRepository : IRepository<ServiceOrder>
{
    Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCompanyAsync(long companyId);
    Task<IEnumerable<ServiceOrder>> SearchServiceOrdersAsync(long companyId, string searchTerm);
    Task<ServiceOrder?> GetServiceOrderWithDetailsAsync(long serviceOrderId);
    Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCustomerAsync(long customerId);
    Task<IEnumerable<ServiceOrder>> GetServiceOrdersByEmployeeAsync(long employeeId);
    Task<IEnumerable<ServiceOrder>> GetServiceOrdersByStatusAsync(long companyId, Domain.Enums.OrderStatus status);
    Task<long> GetLastOrderIdAsync();
}
