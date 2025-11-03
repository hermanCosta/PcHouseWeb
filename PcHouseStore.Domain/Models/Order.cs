using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("order")]
public class Order
{
    [Key]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Required]
    [MaxLength(40)]
    [Column("order_number")]
    public string OrderNumber { get; set; } = string.Empty;

    [Column("customer_id")]
    public long? CustomerId { get; set; }

    [Required]
    [Column("order_type")]
    public OrderType OrderType { get; set; } = OrderType.Retail;

    [Required]
    [Column("status")]
    public OrderStatus Status { get; set; } = OrderStatus.Created;

    [Required]
    [MaxLength(3)]
    [Column("currency")]
    public string Currency { get; set; } = "EUR";

    [Required]
    [Precision(12, 2)]
    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("total_vat_amount")]
    public decimal TotalVatAmount { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("balance_due")]
    public decimal BalanceDue { get; set; }

    [Required]
    [Column("placed_at")]
    public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

    [Column("closed_at")]
    public DateTime? ClosedAt { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Required]
    [Column("created_by")]
    public long CreatedByEmployeeId { get; set; }

    public Company Company { get; set; } = null!;
    public Customer? Customer { get; set; }
    public Employee CreatedBy { get; set; } = null!;
    public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
    public ICollection<OrderStatusEvent> StatusHistory { get; set; } = new List<OrderStatusEvent>();
    public ICollection<OrderNote> NotesHistory { get; set; } = new List<OrderNote>();
    public ICollection<OrderFault> Faults { get; set; } = new List<OrderFault>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}
