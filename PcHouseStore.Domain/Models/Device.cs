using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("DEVICE")]
    public class Device
    {
        [Key]
        [Column("ID_DEVICE")]
        public long DeviceId { get; set; }

        [Required]
        [Column("BRAND")]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [Column("MODEL")]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Column("SERIAL_NUMBER")]
        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        public string DisplayName => $"{Brand} {Model}".Trim();

        // Navigation properties
        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
    }
}