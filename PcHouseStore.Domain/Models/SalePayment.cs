using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models
{
    [Table("SALE_PAYMENT")]
    public class SalePayment
    {
        [Key]
        [Column("ID_SALE_PAYMENT")]
        public long SalePaymentId { get; set; }

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        [Column("ID_SALE")]
        public long SaleId { get; set; }

        [Column("PAYMENT_TYPE")]
        public PaymentType? PaymentType { get; set; }

        [Required]
        [Column("PAY_METHOD")]
        public PayMethod PayMethod { get; set; }

        [Column("AMOUNT_DUE")]
        public double? AmountDue { get; set; }

        [Column("AMOUNT_PAID")]
        public double? AmountPaid { get; set; }

        [Column("CARD_AMOUNT")]
        public double? CardAmount { get; set; }

        [Column("CASH_AMOUNT")]
        public double? CashAmount { get; set; }

        [Column("CHANGE_AMOUNT")]
        public double? ChangeAmount { get; set; }

        [Column("DT_TRANSACTION")]
        public DateTime? DtTransaction { get; set; }

        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public Sale Sale { get; set; } = null!;
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

        // Validation
        public void SetAmountDue(double? amountDue)
        {
            if (amountDue.HasValue && amountDue < 0)
                throw new ArgumentException("Amount due cannot be negative.");
            AmountDue = amountDue;
        }

        public void SetAmountPaid(double? amountPaid)
        {
            if (amountPaid.HasValue && amountPaid < 0)
                throw new ArgumentException("Amount paid cannot be negative.");
            AmountPaid = amountPaid;
        }

        public void SetChangeAmount(double? changeAmount)
        {
            if (changeAmount.HasValue && changeAmount < 0)
                throw new ArgumentException("Change amount cannot be negative.");
            ChangeAmount = changeAmount;
        }
    }
}