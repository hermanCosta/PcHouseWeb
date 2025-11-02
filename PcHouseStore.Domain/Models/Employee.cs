using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("employee")]
public class Employee
{
    [Key]
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Required]
    [Column("person_id")]
    public long PersonId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Required]
    [MaxLength(60)]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Column("role")]
    public EmployeeRole Role { get; set; } = EmployeeRole.Sales;

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("hired_on")]
    public DateTime? HiredOn { get; set; }

    [Column("terminated_on")]
    public DateTime? TerminatedOn { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Person Person { get; set; } = null!;
    public Company Company { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<ServiceOrder> AssignedServiceOrders { get; set; } = new List<ServiceOrder>();
    public ICollection<OrderStatusEvent> OrderStatusEvents { get; set; } = new List<OrderStatusEvent>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<CashMovement> CashMovements { get; set; } = new List<CashMovement>();
    public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
    public ICollection<DailyClosing> DailyClosings { get; set; } = new List<DailyClosing>();
    public ICollection<OrderNote> OrderNotes { get; set; } = new List<OrderNote>();
}