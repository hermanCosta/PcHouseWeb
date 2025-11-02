using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.Infrastructure.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByCompanyAsync(long companyId, OrderType? orderType = null);
    Task<IEnumerable<Order>> SearchOrdersAsync(long companyId, string searchTerm, OrderType? orderType = null);
    Task<Order?> GetOrderWithDetailsAsync(long orderId);
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(long customerId, OrderType? orderType = null);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(long companyId, DateTime startDate, DateTime endDate, OrderType? orderType = null);
}
