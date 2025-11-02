using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("CUSTOMER")]
    public class Customer
    {
        [Key]
        [Column("ID_CUSTOMER")]
        public long CustomerId { get; set; }

        [Column("ID_PERSON")]
        public long PersonId { get; set; }

        [Column("ID_COMPANY")]
        public long? CompanyId { get; set; }

        // Navigation properties
        public Person Person { get; set; } = null!;
        public Company? Company { get; set; }
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
    }
}