using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("order_fault")]
public class OrderFault
{
    [Column("order_id")]
    public long OrderId { get; set; }

    [Column("fault_id")]
    public long FaultId { get; set; }

    public Order Order { get; set; } = null!;
    public Fault Fault { get; set; } = null!;
}
