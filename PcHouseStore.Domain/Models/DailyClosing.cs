using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PcHouseStore.Domain.Models;

[Table("daily_closing")]
public class DailyClosing
{
    [Key]
    [Column("daily_closing_id")]
    public long DailyClosingId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Column("location_id")]
    public long? LocationId { get; set; }

    [Required]
    [Column("closing_date")]
    public DateOnly ClosingDate { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("total_cash")]
    public decimal TotalCash { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("total_card")]
    public decimal TotalCard { get; set; }

    [Required]
    [Column("total_orders")]
    public int TotalOrders { get; set; }

    [Required]
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Company Company { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
}
