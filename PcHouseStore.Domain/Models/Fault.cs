using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("FAULT")]
    public class Fault
    {
        [Key]
        [Column("ID_FAULT")]
        public long FaultId { get; set; }

        [Column("CODE")]
        [MaxLength(50)]
        public string? Code { get; set; }

        [Required]
        [Column("DESCRIPTION")]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<ServiceOrderFault> ServiceOrderFaults { get; set; } = new List<ServiceOrderFault>();
    }
}