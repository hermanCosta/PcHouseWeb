using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("SERVICE_ORDER_FAULT")]
    public class ServiceOrderFault
    {
        [Key]
        [Column("ID_SERVICE_ORDER_FAULT")]
        public long ServiceOrderFaultId { get; set; }

        [Column("ID_SERVICE_ORDER")]
        public long ServiceOrderId { get; set; }

        [Column("ID_FAULT")]
        public long FaultId { get; set; }

        // Navigation properties
        public ServiceOrder ServiceOrder { get; set; } = null!;
        public Fault Fault { get; set; } = null!;
    }
}