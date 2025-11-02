using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.Infrastructure.Data;

public class PcHouseStoreDbContext : DbContext
{
    public PcHouseStoreDbContext(DbContextOptions<PcHouseStoreDbContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<PersonAddress> PersonAddresses => Set<PersonAddress>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Fault> Faults => Set<Fault>();

    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();
    public DbSet<PriceBook> PriceBooks => Set<PriceBook>();
    public DbSet<PriceBookItem> PriceBookItems => Set<PriceBookItem>();
    public DbSet<RefurbTemplate> RefurbTemplates => Set<RefurbTemplate>();
    public DbSet<RefurbAttribute> RefurbAttributes => Set<RefurbAttribute>();
    public DbSet<RefurbItem> RefurbItems => Set<RefurbItem>();
    public DbSet<RefurbAttributeValue> RefurbAttributeValues => Set<RefurbAttributeValue>();

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();
    public DbSet<RetailOrder> RetailOrders => Set<RetailOrder>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<OrderStatusEvent> OrderStatusEvents => Set<OrderStatusEvent>();
    public DbSet<OrderNote> OrderNotes => Set<OrderNote>();
    public DbSet<OrderFault> OrderFaults => Set<OrderFault>();

    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<CashMovement> CashMovements => Set<CashMovement>();
    public DbSet<DailyClosing> DailyClosings => Set<DailyClosing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>().UseTptMappingStrategy();

        modelBuilder.Entity<PersonAddress>()
            .HasKey(pa => new { pa.PersonId, pa.AddressId, pa.UsageType });

        modelBuilder.Entity<OrderFault>()
            .HasKey(of => new { of.OrderId, of.FaultId });

        // Address configuration
        modelBuilder.Entity<Address>()
            .Property(a => a.CountryIso2)
            .HasConversion(v => v.ToUpperInvariant(), v => v);

        modelBuilder.Entity<Company>()
            .HasOne(c => c.BillingAddress)
            .WithMany(a => a.BillingCompanies)
            .HasForeignKey(c => c.BillingAddressId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Company>()
            .HasOne(c => c.ShippingAddress)
            .WithMany(a => a.ShippingCompanies)
            .HasForeignKey(c => c.ShippingAddressId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Company>()
            .HasIndex(c => new { c.LegalName, c.TradingName });

        modelBuilder.Entity<Person>()
            .HasIndex(p => new { p.FirstName, p.LastName });

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Person)
            .WithMany(p => p.Customers)
            .HasForeignKey(c => c.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Company)
            .WithMany(co => co.Customers)
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Employee>()
            .Property(e => e.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Username)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Person)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Company)
            .WithMany(c => c.Employees)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Device>()
            .HasOne(d => d.Customer)
            .WithMany(c => c.Devices)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Device>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();

        modelBuilder.Entity<CatalogItem>()
            .Property(ci => ci.ItemType)
            .HasConversion<string>();

        modelBuilder.Entity<CatalogItem>()
            .HasOne(ci => ci.Company)
            .WithMany(c => c.CatalogItems)
            .HasForeignKey(ci => ci.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CatalogItem>()
            .HasIndex(ci => new { ci.CompanyId, ci.Sku })
            .IsUnique();

        modelBuilder.Entity<PriceBook>()
            .HasOne(pb => pb.Company)
            .WithMany(c => c.PriceBooks)
            .HasForeignKey(pb => pb.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PriceBookItem>()
            .HasOne(pbi => pbi.PriceBook)
            .WithMany(pb => pb.Items)
            .HasForeignKey(pbi => pbi.PriceBookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PriceBookItem>()
            .HasOne(pbi => pbi.CatalogItem)
            .WithMany(ci => ci.PriceBookItems)
            .HasForeignKey(pbi => pbi.CatalogItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PriceBookItem>()
            .HasIndex(pbi => new { pbi.PriceBookId, pbi.CatalogItemId })
            .IsUnique();

        modelBuilder.Entity<RefurbTemplate>()
            .HasOne(rt => rt.CatalogItem)
            .WithOne(ci => ci.RefurbTemplate)
            .HasForeignKey<RefurbTemplate>(rt => rt.CatalogItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefurbAttribute>()
            .Property(ra => ra.DataType)
            .HasConversion<string>();

        modelBuilder.Entity<RefurbAttribute>()
            .HasOne(ra => ra.RefurbTemplate)
            .WithMany(rt => rt.Attributes)
            .HasForeignKey(ra => ra.RefurbTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefurbItem>()
            .Property(ri => ri.ConditionGrade)
            .HasConversion<string>();

        modelBuilder.Entity<RefurbItem>()
            .Property(ri => ri.Status)
            .HasConversion<string>();

        modelBuilder.Entity<RefurbItem>()
            .HasOne(ri => ri.RefurbTemplate)
            .WithMany(rt => rt.RefurbItems)
            .HasForeignKey(ri => ri.RefurbTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefurbItem>()
            .HasOne(ri => ri.Company)
            .WithMany(c => c.RefurbItems)
            .HasForeignKey(ri => ri.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefurbItem>()
            .HasIndex(ri => ri.SerialNumber)
            .IsUnique();

        modelBuilder.Entity<RefurbAttributeValue>()
            .HasOne(rv => rv.RefurbItem)
            .WithMany(ri => ri.AttributeValues)
            .HasForeignKey(rv => rv.RefurbItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefurbAttributeValue>()
            .HasOne(rv => rv.RefurbAttribute)
            .WithMany(ra => ra.Values)
            .HasForeignKey(rv => rv.RefurbAttributeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefurbAttributeValue>()
            .HasIndex(rv => new { rv.RefurbItemId, rv.RefurbAttributeId })
            .IsUnique();

        modelBuilder.Entity<Order>()
            .Property(o => o.OrderType)
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Company)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.CreatedBy)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.CreatedByEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasIndex(o => new { o.CompanyId, o.OrderNumber })
            .IsUnique();

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Device)
            .WithMany(d => d.ServiceOrders)
            .HasForeignKey(so => so.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Technician)
            .WithMany(e => e.AssignedServiceOrders)
            .HasForeignKey(so => so.TechnicianId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<RetailOrder>()
            .Property(ro => ro.SalesChannel)
            .HasConversion<string>();

        modelBuilder.Entity<OrderLine>()
            .Property(ol => ol.FulfilmentStatus)
            .HasConversion<string>();

        modelBuilder.Entity<OrderLine>()
            .HasOne(ol => ol.Order)
            .WithMany(o => o.Lines)
            .HasForeignKey(ol => ol.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderLine>()
            .HasOne(ol => ol.CatalogItem)
            .WithMany(ci => ci.OrderLines)
            .HasForeignKey(ol => ol.CatalogItemId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderLine>()
            .HasOne(ol => ol.RefurbItem)
            .WithMany(ri => ri.OrderLines)
            .HasForeignKey(ol => ol.RefurbItemId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<OrderStatusEvent>()
            .Property(ose => ose.Status)
            .HasConversion<string>();

        modelBuilder.Entity<OrderStatusEvent>()
            .HasOne(ose => ose.Order)
            .WithMany(o => o.StatusHistory)
            .HasForeignKey(ose => ose.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderStatusEvent>()
            .HasOne(ose => ose.ChangedBy)
            .WithMany(e => e.OrderStatusEvents)
            .HasForeignKey(ose => ose.ChangedByEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderNote>()
            .HasOne(on => on.Order)
            .WithMany(o => o.NotesHistory)
            .HasForeignKey(on => on.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderNote>()
            .HasOne(on => on.Employee)
            .WithMany(e => e.OrderNotes)
            .HasForeignKey(on => on.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderFault>()
            .HasOne(of => of.Order)
            .WithMany(o => o.Faults)
            .HasForeignKey(of => of.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderFault>()
            .HasOne(of => of.Fault)
            .WithMany(f => f.OrderFaults)
            .HasForeignKey(of => of.FaultId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentMethod>()
            .Property(pm => pm.MethodCode)
            .HasConversion<string>();

        modelBuilder.Entity<PaymentMethod>()
            .HasOne(pm => pm.Company)
            .WithMany(c => c.PaymentMethods)
            .HasForeignKey(pm => pm.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentType)
            .HasConversion<string>();

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Company)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.PaymentMethod)
            .WithMany(pm => pm.Payments)
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Employee)
            .WithMany(e => e.Payments)
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashMovement>()
            .Property(cm => cm.MovementType)
            .HasConversion<string>();

        modelBuilder.Entity<CashMovement>()
            .HasOne(cm => cm.Company)
            .WithMany(c => c.CashMovements)
            .HasForeignKey(cm => cm.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashMovement>()
            .HasOne(cm => cm.Employee)
            .WithMany(e => e.CashMovements)
            .HasForeignKey(cm => cm.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashMovement>()
            .HasOne(cm => cm.RelatedPayment)
            .WithMany(p => p.CashMovements)
            .HasForeignKey(cm => cm.RelatedPaymentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Refund>()
            .Property(r => r.ReasonCode)
            .HasConversion<string>();

        modelBuilder.Entity<Refund>()
            .HasOne(r => r.Order)
            .WithMany(o => o.Refunds)
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Refund>()
            .HasOne(r => r.Payment)
            .WithOne(p => p.Refund)
            .HasForeignKey<Refund>(r => r.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refund>()
            .HasOne(r => r.ProcessedBy)
            .WithMany(e => e.Refunds)
            .HasForeignKey(r => r.ProcessedByEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DailyClosing>()
            .HasOne(dc => dc.Company)
            .WithMany(c => c.DailyClosings)
            .HasForeignKey(dc => dc.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DailyClosing>()
            .HasOne(dc => dc.Employee)
            .WithMany(e => e.DailyClosings)
            .HasForeignKey(dc => dc.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DailyClosing>()
            .HasIndex(dc => new { dc.CompanyId, dc.LocationId, dc.ClosingDate })
            .IsUnique();
    }
}