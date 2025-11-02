using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("DEPOSIT")]
    public class Deposit
    {
        [Key]
        [Column("ID_DEPOSIT")]
        public long DepositId { get; set; }

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; } = 1;

        [Column("ID_SERVICE_ORDER")]
        public long? ServiceOrderId { get; set; }

        [Column("ID_SALE")]
        public long? SaleId { get; set; }

        [Column("ID_SERVICE_ORDER_PAYMENT")]
        public long? ServiceOrderPaymentId { get; set; }

        [Column("ID_SALE_PAYMENT")]
        public long? SalePaymentId { get; set; }

        [Required]
        [Column("AMOUNT")]
        public double Amount { get; set; }

        [Required]
        [Column("DT_CREATED")]
        public DateTime DtCreated { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public ServiceOrder? ServiceOrder { get; set; }
        public Sale? Sale { get; set; }
        public ServiceOrderPayment? ServiceOrderPayment { get; set; }
        public SalePayment? SalePayment { get; set; }

        // Validation
        public void SetAmount(double amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.");
            Amount = amount;
        }
    }
}