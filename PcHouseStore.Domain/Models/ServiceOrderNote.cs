using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("SERVICE_ORDER_NOTE")]
    public class ServiceOrderNote
    {
        [Key]
        [Column("ID_SERVICE_ORDER_NOTE")]
        public long ServiceOrderNoteId { get; set; }

        [Column("ID_EMPLOYEE")]
        public long EmployeeId { get; set; }

        [Column("ID_SERVICE_ORDER")]
        public long? ServiceOrderId { get; set; }

        [Column("ID_SALE")]
        public long? SaleId { get; set; }

        [Required]
        [Column("NOTE")]
        [MaxLength(1000)]
        public string Note { get; set; } = string.Empty;

        [Required]
        [Column("CREATED")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public ServiceOrder? ServiceOrder { get; set; }
        public Sale? Sale { get; set; }
    }
}
