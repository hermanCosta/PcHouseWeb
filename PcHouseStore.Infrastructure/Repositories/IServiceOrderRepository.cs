using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.Infrastructure.Repositories;

public interface IServiceOrderRepository : IRepository<ServiceOrder>
{
    Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCompanyAsync(long companyId, OrderStatus? status = null);
    Task<IEnumerable<ServiceOrder>> SearchServiceOrdersAsync(long companyId, string searchTerm);
    Task<ServiceOrder?> GetServiceOrderWithDetailsAsync(long serviceOrderId);
    Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCustomerAsync(long customerId);
    Task<IEnumerable<ServiceOrder>> GetServiceOrdersByTechnicianAsync(long technicianId);
}
