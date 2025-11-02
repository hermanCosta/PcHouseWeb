using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("CASH_OUT_REGISTRY")]
    public class CashOutRegistry
    {
        [Key]
        [Column("ID_CASH_OUT_REGISTRY")]
        public long CashOutRegistryId { get; set; }

        [Required]
        [Column("AMOUNT")]
        public double Amount { get; set; }

        [Required]
        [Column("NOTE")]
        [MaxLength(300)]
        public string Note { get; set; } = string.Empty;

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        [Column("ID_COMPANY")]
        public long CompanyId { get; set; }

        [Column("DT_TRANSACTION")]
        public DateTime DtTransaction { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public Company Company { get; set; } = null!;
    }
}
