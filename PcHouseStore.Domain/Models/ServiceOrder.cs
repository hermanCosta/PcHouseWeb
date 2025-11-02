using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models
{
    [Table("SERVICE_ORDER")]
    public class ServiceOrder
    {
        [Key]
        [Column("ID_SERVICE_ORDER")]
        public long ServiceOrderId { get; set; }

        [Column("ID_CUSTOMER")]
        public long CustomerId { get; set; }

        [Column("ID_DEVICE")]
        public long? DeviceId { get; set; }

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        [Column("ID_COMPANY")]
        public long CompanyId { get; set; }

        [Column("TOTAL")]
        public double? Total { get; set; }

        [Column("DUE")]
        public double? Due { get; set; }

        [Required]
        [Column("STATUS")]
        public OrderStatus Status { get; set; } = OrderStatus.Created;

        [Required]
        [Column("CREATED")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Column("FINISHED")]
        public DateTime? Finished { get; set; }

        [Column("PICKED")]
        public DateTime? Picked { get; set; }

        [Column("BAD_SECTOR")]
        public int BadSector { get; set; } = 0;

        [Column("NOTE")]
        [MaxLength(500)]
        public string? Note { get; set; }

        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public Device? Device { get; set; }
        public Employee Employee { get; set; } = null!;
        public Company Company { get; set; } = null!;
        public ICollection<ServiceOrderPayment> ServiceOrderPayments { get; set; } = new List<ServiceOrderPayment>();
        public ICollection<ServiceOrderFault> ServiceOrderFaults { get; set; } = new List<ServiceOrderFault>();
        public ICollection<ServiceOrderProdServ> ServiceOrderProdServs { get; set; } = new List<ServiceOrderProdServ>();
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
        public ICollection<ServiceOrderNote> ServiceOrderNotes { get; set; } = new List<ServiceOrderNote>();

        // Validation
        public void SetTotal(double? total)
        {
            if (total.HasValue && total < 0)
                throw new ArgumentException("Total cannot be negative.");
            Total = total;
        }

        public void SetDue(double? due)
        {
            if (due.HasValue && due < 0)
                throw new ArgumentException("Due cannot be negative.");
            Due = due;
        }

        public void SetBadSector(int badSector)
        {
            if (badSector < 0)
                throw new ArgumentException("Bad sector cannot be negative.");
            BadSector = badSector;
        }
    }
}