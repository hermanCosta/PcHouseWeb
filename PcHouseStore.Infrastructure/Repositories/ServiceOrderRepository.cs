using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;

namespace PcHouseStore.Infrastructure.Repositories;

public class ServiceOrderRepository : Repository<ServiceOrder>, IServiceOrderRepository
{
    public ServiceOrderRepository(PcHouseStoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCompanyAsync(long companyId, OrderStatus? status = null)
    {
        return await BuildServiceOrderQuery(includeLines: true, includeNotes: false)
            .Where(so => so.CompanyId == companyId && (!status.HasValue || so.Status == status))
            .OrderByDescending(so => so.PlacedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> SearchServiceOrdersAsync(long companyId, string searchTerm)
    {
        searchTerm = searchTerm.Trim();

        return await BuildServiceOrderQuery(includeLines: true, includeNotes: false)
            .Where(so => so.CompanyId == companyId &&
                        (
                            (so.Customer != null && so.Customer.Person.FirstName.Contains(searchTerm)) ||
                            (so.Customer != null && so.Customer.Person.LastName.Contains(searchTerm)) ||
                            so.OrderNumber.Contains(searchTerm) ||
                            (so.Device != null && (so.Device.Brand.Contains(searchTerm) || so.Device.Model.Contains(searchTerm) || (so.Device.SerialNumber != null && so.Device.SerialNumber.Contains(searchTerm)))) ||
                            (so.Notes != null && so.Notes.Contains(searchTerm)) ||
                            (so.CheckInNotes != null && so.CheckInNotes.Contains(searchTerm))
                        ))
            .OrderByDescending(so => so.PlacedAt)
            .ToListAsync();
    }

    public async Task<ServiceOrder?> GetServiceOrderWithDetailsAsync(long serviceOrderId)
    {
        return await BuildServiceOrderQuery(includeLines: true, includeNotes: true, includePayments: true)
            .FirstOrDefaultAsync(so => so.OrderId == serviceOrderId);
    }

    public async Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCustomerAsync(long customerId)
    {
        return await BuildServiceOrderQuery(includeLines: true, includeNotes: false)
            .Where(so => so.CustomerId == customerId)
            .OrderByDescending(so => so.PlacedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetServiceOrdersByTechnicianAsync(long technicianId)
    {
        return await BuildServiceOrderQuery(includeLines: true, includeNotes: false)
            .Where(so => so.TechnicianId == technicianId)
            .OrderByDescending(so => so.PlacedAt)
            .ToListAsync();
    }

    private IQueryable<ServiceOrder> BuildServiceOrderQuery(bool includeLines, bool includeNotes, bool includePayments = false)
    {
        IQueryable<ServiceOrder> query = _dbSet
            .Include(so => so.Company)
            .Include(so => so.Customer)!
                .ThenInclude(c => c.Person)
            .Include(so => so.CreatedBy)
                .ThenInclude(e => e.Person)
            .Include(so => so.StatusHistory)
                .ThenInclude(sh => sh.ChangedBy)
                    .ThenInclude(e => e.Person)
            .Include(so => so.Device)
            .Include(so => so.Technician)
                .ThenInclude(e => e.Person)
            .Include(so => so.Faults)
                .ThenInclude(of => of.Fault);

        if (includeLines)
        {
            query = query
                .Include(so => so.Lines)
                    .ThenInclude(ol => ol.CatalogItem)
                .Include(so => so.Lines)
                    .ThenInclude(ol => ol.RefurbItem);
        }

        if (includePayments)
        {
            query = query
                .Include(so => so.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(so => so.Refunds)
                    .ThenInclude(r => r.Payment);
        }

        if (includeNotes)
        {
            query = query
                .Include(so => so.NotesHistory)
                    .ThenInclude(n => n.Employee)
                        .ThenInclude(e => e.Person);
        }

        return query.AsSplitQuery();
    }
}
