using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("DAILY_CLOSING")]
    public class DailyClosing
    {
        [Key]
        [Column("ID_DAILY_CLOSING")]
        public long DailyClosingId { get; set; }

        [Required]
        [Column("CARD_TOTAL")]
        public double CardTotal { get; set; }

        [Required]
        [Column("CASH_TOTAL")]
        public double CashTotal { get; set; }

        [Required]
        [Column("CLOSING_DATE")]
        public DateOnly ClosingDate { get; set; }

        [Required]
        [Column("CLOSING_DATETIME")]
        public DateTime ClosingDateTime { get; set; }

        [Column("NOTES")]
        public string? Notes { get; set; }

        [Required]
        [Column("TOTAL_TRANSACTIONS")]
        public int TotalTransactions { get; set; }

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        // Navigation properties
        public Employee Employee { get; set; } = null!;
    }
}
