using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("cash_movement")]
public class CashMovement
{
    [Key]
    [Column("cash_movement_id")]
    public long CashMovementId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Required]
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Required]
    [Column("movement_type")]
    public CashMovementType MovementType { get; set; } = CashMovementType.CashIn;

    [Required]
    [Precision(12, 2)]
    [Column("amount")]
    public decimal Amount { get; set; }

    [MaxLength(300)]
    [Column("reason")]
    public string? Reason { get; set; }

    [Column("related_payment_id")]
    public long? RelatedPaymentId { get; set; }

    [Required]
    [Column("occurred_at")]
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public Company Company { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    public Payment? RelatedPayment { get; set; }
}
