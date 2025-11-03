using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("refund")]
public class Refund
{
    [Key]
    [Column("refund_id")]
    public long RefundId { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("payment_id")]
    public long PaymentId { get; set; }

    [Required]
    [Column("reason_code")]
    public RefundReason ReasonCode { get; set; } = RefundReason.Other;

    [Required]
    [Precision(12, 2)]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Required]
    [Column("processed_by")]
    public long ProcessedByEmployeeId { get; set; }

    [Required]
    [Column("processed_at")]
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

    public Order Order { get; set; } = null!;
    public Payment Payment { get; set; } = null!;
    public Employee ProcessedBy { get; set; } = null!;
}