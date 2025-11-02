using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PcHouseStore.Domain.Models;

[Table("device")]
public class Device
{
    [Key]
    [Column("device_id")]
    public long DeviceId { get; set; }

    [Required]
    [Column("customer_id")]
    public long CustomerId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("brand")]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("model")]
    public string Model { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("serial_number")]
    public string? SerialNumber { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

    public string DisplayName => string.Join(" ", new[] { Brand, Model }.Where(s => !string.IsNullOrWhiteSpace(s)));
}