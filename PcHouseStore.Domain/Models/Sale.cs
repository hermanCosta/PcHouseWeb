using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models
{
    [Table("SALE")]
    public class Sale
    {
        [Key]
        [Column("ID_SALE")]
        public long SaleId { get; set; }

        [Column("ID_CUSTOMER")]
        public long CustomerId { get; set; }

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        [Column("ID_COMPANY")]
        public long CompanyId { get; set; }

        [Required]
        [Column("TOTAL")]
        public double Total { get; set; }

        [Required]
        [Column("REMAINING")]
        public double Remaining { get; set; }

        [Required]
        [Column("CREATED")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("STATUS")]
        public OrderStatus Status { get; set; } = OrderStatus.Created;

        [Column("IMPORTANT_NOTES")]
        [MaxLength(500)]
        public string? ImportantNotes { get; set; }

        [Required]
        [Column("SALE_TYPE")]
        public SaleType SaleType { get; set; } = SaleType.Common;

        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public Company Company { get; set; } = null!;
        public ICollection<SalePayment> Payments { get; set; } = new List<SalePayment>();
        public ICollection<SaleProdServ> SaleProductServices { get; set; } = new List<SaleProdServ>();
        public ICollection<RefurbSale> RefurbSales { get; set; } = new List<RefurbSale>();
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
        public ICollection<ServiceOrderNote> ServiceOrderNotes { get; set; } = new List<ServiceOrderNote>();

        // Validation
        public void SetTotal(double total)
        {
            if (total < 0)
                throw new ArgumentException("Total cannot be negative.");
            Total = total;
        }

        public void SetRemaining(double remaining)
        {
            if (remaining < 0)
                throw new ArgumentException("Remaining cannot be negative.");
            Remaining = remaining;
        }
    }
}