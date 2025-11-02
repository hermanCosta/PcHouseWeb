using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Models;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Infrastructure.Data;

public class PcHouseStoreDbContext : DbContext
{
    public PcHouseStoreDbContext(DbContextOptions<PcHouseStoreDbContext> options) : base(options)
    {
    }

    // Core entities
    public DbSet<Company> Companies { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<ProductService> ProductServices { get; set; }
    public DbSet<Fault> Faults { get; set; }

    // Business entities
    public DbSet<Sale> Sales { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<ServiceOrderNote> ServiceOrderNotes { get; set; }

    // Payment entities
    public DbSet<SalePayment> SalePayments { get; set; }
    public DbSet<ServiceOrderPayment> ServiceOrderPayments { get; set; }
    public DbSet<Deposit> Deposits { get; set; }
    public DbSet<Refund> Refunds { get; set; }

    // Refurb entities
    public DbSet<Refurb> Refurbs { get; set; }
    public DbSet<RefurbSale> RefurbSales { get; set; }

    // Cash management entities
    public DbSet<CashInRegistry> CashInRegistries { get; set; }
    public DbSet<CashOutRegistry> CashOutRegistries { get; set; }
    public DbSet<DailyClosing> DailyClosings { get; set; }

    // Junction tables
    public DbSet<SaleProdServ> SaleProdServs { get; set; }
    public DbSet<ServiceOrderProdServ> ServiceOrderProdServs { get; set; }
    public DbSet<ServiceOrderFault> ServiceOrderFaults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure enums
        modelBuilder.Entity<Sale>()
            .Property(s => s.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Sale>()
            .Property(s => s.SaleType)
            .HasConversion<string>();

        modelBuilder.Entity<ServiceOrder>()
            .Property(so => so.Status)
            .HasConversion<string>();

        modelBuilder.Entity<SalePayment>()
            .Property(sp => sp.PaymentType)
            .HasConversion<string>();

        modelBuilder.Entity<SalePayment>()
            .Property(sp => sp.PayMethod)
            .HasConversion<string>();

        modelBuilder.Entity<ServiceOrderPayment>()
            .Property(sop => sop.PaymentType)
            .HasConversion<string>();

        modelBuilder.Entity<ServiceOrderPayment>()
            .Property(sop => sop.PayMethod)
            .HasConversion<string>();

        // Configure decimal precision
        modelBuilder.Entity<Sale>()
            .Property(s => s.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Sale>()
            .Property(s => s.Remaining)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ServiceOrder>()
            .Property(so => so.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ServiceOrder>()
            .Property(so => so.Due)
            .HasPrecision(18, 2);

        // Configure indexes
        modelBuilder.Entity<Sale>()
            .HasIndex(s => s.Created);

        modelBuilder.Entity<ServiceOrder>()
            .HasIndex(so => so.Created);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PersonId);

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.PersonId);

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Username)
            .IsUnique();

        modelBuilder.Entity<Device>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();

        modelBuilder.Entity<Fault>()
            .HasIndex(f => f.Code)
            .IsUnique();

        modelBuilder.Entity<Refurb>()
            .HasIndex(r => r.SerialNumber)
            .IsUnique();

        modelBuilder.Entity<DailyClosing>()
            .HasIndex(dc => dc.ClosingDate)
            .IsUnique();

        // Configure relationships
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Person)
            .WithMany()
            .HasForeignKey(c => c.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Company)
            .WithMany(co => co.Customers)
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Person)
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Employee)
            .WithMany(e => e.Sales)
            .HasForeignKey(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Company)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Customer)
            .WithMany(c => c.ServiceOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Device)
            .WithMany(d => d.ServiceOrders)
            .HasForeignKey(so => so.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Employee)
            .WithMany(e => e.ServiceOrders)
            .HasForeignKey(so => so.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Company)
            .WithMany(c => c.ServiceOrders)
            .HasForeignKey(so => so.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductService>()
            .HasOne(ps => ps.Company)
            .WithMany(c => c.ProductServices)
            .HasForeignKey(ps => ps.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refurb>()
            .HasOne(r => r.Company)
            .WithMany(c => c.Refurbs)
            .HasForeignKey(r => r.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SaleProdServ>()
            .HasOne(sps => sps.Sale)
            .WithMany(s => s.SaleProductServices)
            .HasForeignKey(sps => sps.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SaleProdServ>()
            .HasOne(sps => sps.ProductService)
            .WithMany(ps => ps.SaleProdServs)
            .HasForeignKey(sps => sps.ProdServId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrderProdServ>()
            .HasOne(sops => sops.ServiceOrder)
            .WithMany(so => so.ServiceOrderProdServs)
            .HasForeignKey(sops => sops.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceOrderProdServ>()
            .HasOne(sops => sops.ProductService)
            .WithMany(ps => ps.ServiceOrderProdServs)
            .HasForeignKey(sops => sops.ProdServId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrderFault>()
            .HasOne(sof => sof.ServiceOrder)
            .WithMany(so => so.ServiceOrderFaults)
            .HasForeignKey(sof => sof.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceOrderFault>()
            .HasOne(sof => sof.Fault)
            .WithMany(f => f.ServiceOrderFaults)
            .HasForeignKey(sof => sof.FaultId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefurbSale>()
            .HasOne(rs => rs.Sale)
            .WithMany(s => s.RefurbSales)
            .HasForeignKey(rs => rs.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefurbSale>()
            .HasOne(rs => rs.Refurb)
            .WithMany(r => r.RefurbSales)
            .HasForeignKey(rs => rs.RefurbId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SalePayment>()
            .HasOne(sp => sp.Employee)
            .WithMany(e => e.SalePayments)
            .HasForeignKey(sp => sp.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalePayment>()
            .HasOne(sp => sp.Sale)
            .WithMany(s => s.Payments)
            .HasForeignKey(sp => sp.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceOrderPayment>()
            .HasOne(sop => sop.Employee)
            .WithMany(e => e.ServiceOrderPayments)
            .HasForeignKey(sop => sop.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrderPayment>()
            .HasOne(sop => sop.ServiceOrder)
            .WithMany(so => so.ServiceOrderPayments)
            .HasForeignKey(sop => sop.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Deposit>()
            .HasOne(d => d.Employee)
            .WithMany(e => e.Deposits)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Deposit>()
            .HasOne(d => d.ServiceOrder)
            .WithMany(so => so.Deposits)
            .HasForeignKey(d => d.ServiceOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Deposit>()
            .HasOne(d => d.Sale)
            .WithMany(s => s.Deposits)
            .HasForeignKey(d => d.SaleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refund>()
            .HasOne(r => r.Company)
            .WithMany(c => c.Refunds)
            .HasForeignKey(r => r.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refund>()
            .HasOne(r => r.Employee)
            .WithMany(e => e.Refunds)
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refund>()
            .HasOne(r => r.ServiceOrder)
            .WithMany(so => so.Refunds)
            .HasForeignKey(r => r.ServiceOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Refund>()
            .HasOne(r => r.Sale)
            .WithMany(s => s.Refunds)
            .HasForeignKey(r => r.SaleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrderNote>()
            .HasOne(son => son.Employee)
            .WithMany(e => e.ServiceOrderNotes)
            .HasForeignKey(son => son.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrderNote>()
            .HasOne(son => son.ServiceOrder)
            .WithMany(so => so.ServiceOrderNotes)
            .HasForeignKey(son => son.ServiceOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrderNote>()
            .HasOne(son => son.Sale)
            .WithMany(s => s.ServiceOrderNotes)
            .HasForeignKey(son => son.SaleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashInRegistry>()
            .HasOne(cir => cir.Employee)
            .WithMany(e => e.CashInRegistries)
            .HasForeignKey(cir => cir.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashInRegistry>()
            .HasOne(cir => cir.Company)
            .WithMany(c => c.CashInRegistries)
            .HasForeignKey(cir => cir.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashOutRegistry>()
            .HasOne(cor => cor.Employee)
            .WithMany(e => e.CashOutRegistries)
            .HasForeignKey(cor => cor.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CashOutRegistry>()
            .HasOne(cor => cor.Company)
            .WithMany(c => c.CashOutRegistries)
            .HasForeignKey(cor => cor.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DailyClosing>()
            .HasOne(dc => dc.Employee)
            .WithMany(e => e.DailyClosings)
            .HasForeignKey(dc => dc.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}