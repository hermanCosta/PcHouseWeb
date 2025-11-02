using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("REFUND")]
    public class Refund
    {
        [Key]
        [Column("ID_REFUND")]
        public long RefundId { get; set; }

        [Column("ID_COMPANY")]
        public long CompanyId { get; set; }

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        [Column("ID_SERVICE_ORDER")]
        public long? ServiceOrderId { get; set; }

        [Column("ID_SALE")]
        public long? SaleId { get; set; }

        [Required]
        [Column("AMOUNT")]
        public double Amount { get; set; }

        [Required]
        [Column("DT_CREATED")]
        public DateTime DtCreated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Company Company { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public ServiceOrder? ServiceOrder { get; set; }
        public Sale? Sale { get; set; }

        // Validation
        public void SetAmount(double amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.");
            Amount = amount;
        }
    }
}