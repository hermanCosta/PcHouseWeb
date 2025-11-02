using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("EMPLOYEE")]
    public class Employee
    {
        [Key]
        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        [Column("ID_PERSON")]
        public long PersonId { get; set; }

        [Required]
        [Column("USERNAME")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("PASSWORD")]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [Column("ACCESS_LEVEL")]
        [MaxLength(255)]
        public string? AccessLevel { get; set; }

        // Navigation properties
        public Person Person { get; set; } = null!;
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
        public ICollection<SalePayment> SalePayments { get; set; } = new List<SalePayment>();
        public ICollection<ServiceOrderPayment> ServiceOrderPayments { get; set; } = new List<ServiceOrderPayment>();
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
        public ICollection<CashInRegistry> CashInRegistries { get; set; } = new List<CashInRegistry>();
        public ICollection<CashOutRegistry> CashOutRegistries { get; set; } = new List<CashOutRegistry>();
        public ICollection<DailyClosing> DailyClosings { get; set; } = new List<DailyClosing>();
        public ICollection<ServiceOrderNote> ServiceOrderNotes { get; set; } = new List<ServiceOrderNote>();
    }
}