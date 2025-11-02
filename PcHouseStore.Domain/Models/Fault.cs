using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("fault")]
public class Fault
{
    [Key]
    [Column("fault_id")]
    public long FaultId { get; set; }

    [MaxLength(50)]
    [Column("code")]
    public string? Code { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    public ICollection<OrderFault> OrderFaults { get; set; } = new List<OrderFault>();
}