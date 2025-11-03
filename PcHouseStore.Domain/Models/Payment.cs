using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("payment")]
public class Payment
{
    [Key]
    [Column("payment_id")]
    public long PaymentId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Column("order_id")]
    public long? OrderId { get; set; }

    [Required]
    [Column("payment_method_id")]
    public long PaymentMethodId { get; set; }

    [Required]
    [Column("payment_type")]
    public PaymentType PaymentType { get; set; } = PaymentType.Balance;

    [Required]
    [Precision(12, 2)]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Precision(12, 2)]
    [Column("net_cash")]
    public decimal NetCash { get; set; }

    [Precision(12, 2)]
    [Column("net_card")]
    public decimal NetCard { get; set; }

    [Precision(12, 2)]
    [Column("net_voucher")]
    public decimal NetVoucher { get; set; }

    [Required]
    [Column("processed_at")]
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [MaxLength(120)]
    [Column("reference")]
    public string? Reference { get; set; }

    [MaxLength(500)]
    [Column("notes")]
    public string? Notes { get; set; }

    public Company Company { get; set; } = null!;
    public Order? Order { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public Refund? Refund { get; set; }
    public ICollection<CashMovement> CashMovements { get; set; } = new List<CashMovement>();
}
