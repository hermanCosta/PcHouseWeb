using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Models;
using PcHouseStore.Infrastructure.Data;

namespace PcHouseStore.Infrastructure.Repositories;

public class ServiceOrderRepository : Repository<ServiceOrder>, IServiceOrderRepository
{
    public ServiceOrderRepository(PcHouseStoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCompanyAsync(long companyId)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c.Person)
            .Include(so => so.Device)
            .Include(so => so.Employee)
                .ThenInclude(e => e.Person)
            .Include(so => so.Company)
            .Include(so => so.ServiceOrderPayments)
            .Include(so => so.ServiceOrderFaults)
                .ThenInclude(sof => sof.Fault)
            .Include(so => so.ServiceOrderProdServs)
                .ThenInclude(sops => sops.ProductService)
            .Where(so => so.CompanyId == companyId)
            .OrderByDescending(so => so.Created)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> SearchServiceOrdersAsync(long companyId, string searchTerm)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c.Person)
            .Include(so => so.Device)
            .Include(so => so.Employee)
                .ThenInclude(e => e.Person)
            .Include(so => so.Company)
            .Where(so => so.CompanyId == companyId &&
                        (so.Customer.Person.FirstName.Contains(searchTerm) ||
                         so.Customer.Person.LastName.Contains(searchTerm) ||
                         so.Customer.Person.ContactNo.Contains(searchTerm) ||
                         so.Device.Brand!.Contains(searchTerm) ||
                         so.Device.Model!.Contains(searchTerm) ||
                         so.Device.SerialNumber!.Contains(searchTerm) ||
                         so.Note!.Contains(searchTerm)))
            .OrderByDescending(so => so.Created)
            .ToListAsync();
    }

    public async Task<ServiceOrder?> GetServiceOrderWithDetailsAsync(long serviceOrderId)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c.Person)
            .Include(so => so.Device)
            .Include(so => so.Employee)
                .ThenInclude(e => e.Person)
            .Include(so => so.Company)
            .Include(so => so.ServiceOrderPayments)
            .Include(so => so.ServiceOrderFaults)
                .ThenInclude(sof => sof.Fault)
            .Include(so => so.ServiceOrderProdServs)
                .ThenInclude(sops => sops.ProductService)
            .FirstOrDefaultAsync(so => so.ServiceOrderId == serviceOrderId);
    }

    public async Task<IEnumerable<ServiceOrder>> GetServiceOrdersByCustomerAsync(long customerId)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c.Person)
            .Include(so => so.Device)
            .Include(so => so.Employee)
                .ThenInclude(e => e.Person)
            .Include(so => so.ServiceOrderPayments)
            .Include(so => so.ServiceOrderFaults)
                .ThenInclude(sof => sof.Fault)
            .Include(so => so.ServiceOrderProdServs)
                .ThenInclude(sops => sops.ProductService)
            .Where(so => so.CustomerId == customerId)
            .OrderByDescending(so => so.Created)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetServiceOrdersByEmployeeAsync(long employeeId)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c.Person)
            .Include(so => so.Device)
            .Include(so => so.Employee)
                .ThenInclude(e => e.Person)
            .Include(so => so.ServiceOrderPayments)
            .Include(so => so.ServiceOrderFaults)
                .ThenInclude(sof => sof.Fault)
            .Include(so => so.ServiceOrderProdServs)
                .ThenInclude(sops => sops.ProductService)
            .Where(so => so.EmployeeId == employeeId)
            .OrderByDescending(so => so.Created)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceOrder>> GetServiceOrdersByStatusAsync(long companyId, Domain.Enums.OrderStatus status)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c.Person)
            .Include(so => so.Device)
            .Include(so => so.Employee)
                .ThenInclude(e => e.Person)
            .Include(so => so.ServiceOrderPayments)
            .Include(so => so.ServiceOrderFaults)
                .ThenInclude(sof => sof.Fault)
            .Include(so => so.ServiceOrderProdServs)
                .ThenInclude(sops => sops.ProductService)
            .Where(so => so.CompanyId == companyId && so.Status == status)
            .OrderByDescending(so => so.Created)
            .ToListAsync();
    }

    public async Task<long> GetLastOrderIdAsync()
    {
        var lastOrder = await _dbSet
            .OrderByDescending(so => so.ServiceOrderId)
            .FirstOrDefaultAsync();

        return lastOrder?.ServiceOrderId ?? 0;
    }
}
