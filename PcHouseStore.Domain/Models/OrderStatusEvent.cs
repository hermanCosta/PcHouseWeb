using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("order_status_event")]
public class OrderStatusEvent
{
    [Key]
    [Column("order_status_event_id")]
    public long OrderStatusEventId { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("status")]
    public OrderStatus Status { get; set; }

    [Required]
    [Column("changed_at")]
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("changed_by")]
    public long ChangedByEmployeeId { get; set; }

    [MaxLength(500)]
    [Column("comment")]
    public string? Comment { get; set; }

    public Order Order { get; set; } = null!;
    public Employee ChangedBy { get; set; } = null!;
}
