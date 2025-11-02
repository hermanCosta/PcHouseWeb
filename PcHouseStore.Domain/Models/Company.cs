using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("COMPANY")]
    public class Company
    {
        [Key]
        [Column("ID_COMPANY")]
        public long CompanyId { get; set; }

        [Required]
        [Column("NAME")]
        [MaxLength(45)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("ADDRESS")]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("CONTACT_ONE")]
        [MaxLength(45)]
        public string ContactOne { get; set; } = string.Empty;

        [Column("CONTACT_TWO")]
        [MaxLength(45)]
        public string? ContactTwo { get; set; }

        [Column("EMAIL")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Column("PASSWORD")]
        [MaxLength(255)]
        public string? Password { get; set; }

        // Navigation properties
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
        public ICollection<ProductService> ProductServices { get; set; } = new List<ProductService>();
        public ICollection<Refurb> Refurbs { get; set; } = new List<Refurb>();
        public ICollection<CashInRegistry> CashInRegistries { get; set; } = new List<CashInRegistry>();
        public ICollection<CashOutRegistry> CashOutRegistries { get; set; } = new List<CashOutRegistry>();
        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
    }
}