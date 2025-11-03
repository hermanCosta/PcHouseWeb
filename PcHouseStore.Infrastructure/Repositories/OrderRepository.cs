using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;

namespace PcHouseStore.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(PcHouseStoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByCompanyAsync(long companyId, OrderType? orderType = null)
    {
        return await BuildOrderQuery()
            .Where(o => o.CompanyId == companyId && (!orderType.HasValue || o.OrderType == orderType))
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> SearchOrdersAsync(long companyId, string searchTerm, OrderType? orderType = null)
    {
        searchTerm = searchTerm.Trim();

        return await BuildOrderQuery()
            .Where(o => o.CompanyId == companyId && (!orderType.HasValue || o.OrderType == orderType) &&
                        (
                            (o.Customer != null && o.Customer.Person.FirstName.Contains(searchTerm)) ||
                            (o.Customer != null && o.Customer.Person.LastName.Contains(searchTerm)) ||
                            o.OrderNumber.Contains(searchTerm) ||
                            (o.Notes != null && o.Notes.Contains(searchTerm))
                        ))
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithDetailsAsync(long orderId)
    {
        return await BuildOrderQuery(includeLines: true, includePayments: true, includeNotes: true)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(long customerId, OrderType? orderType = null)
    {
        return await BuildOrderQuery()
            .Where(o => o.CustomerId == customerId && (!orderType.HasValue || o.OrderType == orderType))
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(long companyId, DateTime startDate, DateTime endDate, OrderType? orderType = null)
    {
        return await BuildOrderQuery()
            .Where(o => o.CompanyId == companyId
                        && o.PlacedAt >= startDate
                        && o.PlacedAt <= endDate
                        && (!orderType.HasValue || o.OrderType == orderType))
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync();
    }

    private IQueryable<Order> BuildOrderQuery(bool includeLines = true, bool includePayments = false, bool includeNotes = false)
    {
        IQueryable<Order> query = _dbSet
            .Include(o => o.Company)
            .Include(o => o.Customer)!
                .ThenInclude(c => c.Person)
            .Include(o => o.CreatedBy)
                .ThenInclude(e => e.Person)
            .Include(o => o.StatusHistory)
                .ThenInclude(sh => sh.ChangedBy)
                    .ThenInclude(e => e.Person)
            .Include(o => o.Faults)
                .ThenInclude(of => of.Fault);

        if (includeLines)
        {
            query = query
                .Include(o => o.Lines)
                    .ThenInclude(ol => ol.CatalogItem)
                .Include(o => o.Lines)
                    .ThenInclude(ol => ol.RefurbItem);
        }

        if (includePayments)
        {
            query = query
                .Include(o => o.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(o => o.Refunds)
                    .ThenInclude(r => r.Payment);
        }

        if (includeNotes)
        {
            query = query
                .Include(o => o.NotesHistory)
                    .ThenInclude(n => n.Employee)
                        .ThenInclude(e => e.Person);
        }

        return query.AsSplitQuery();
    }
}
