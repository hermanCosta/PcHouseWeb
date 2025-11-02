using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("REFURB")]
    public class Refurb
    {
        [Key]
        [Column("ID_REFURB")]
        public long RefurbId { get; set; }

        [Required]
        [Column("CATEGORY")]
        [MaxLength(45)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Column("BRAND")]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [Column("MODEL")]
        [MaxLength(150)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Column("PRICE")]
        public double Price { get; set; }

        [Column("QTY")]
        public int Qty { get; set; } = 1;

        [Column("SERIAL_NUMBER")]
        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        [Column("NOTE")]
        [MaxLength(400)]
        public string? Note { get; set; }

        [Column("SCREEN")]
        [MaxLength(150)]
        public string? Screen { get; set; }

        [Column("PROCESSOR")]
        [MaxLength(150)]
        public string? Processor { get; set; }

        [Column("RAM_MEMORY")]
        [MaxLength(150)]
        public string? RamMemory { get; set; }

        [Column("STORAGE")]
        [MaxLength(150)]
        public string? Storage { get; set; }

        [Column("GPU_BOARD")]
        [MaxLength(150)]
        public string? GpuBoard { get; set; }

        [Column("BATTERY_HEALTH")]
        [MaxLength(45)]
        public string? BatteryHealth { get; set; }

        [Column("CUSTOM_1")]
        [MaxLength(100)]
        public string? Custom1 { get; set; }

        [Column("CUSTOM_2")]
        [MaxLength(100)]
        public string? Custom2 { get; set; }

        [Column("CUSTOM_3")]
        [MaxLength(100)]
        public string? Custom3 { get; set; }

        [Column("CUSTOM_4")]
        [MaxLength(100)]
        public string? Custom4 { get; set; }

        [Column("CUSTOM_5")]
        [MaxLength(100)]
        public string? Custom5 { get; set; }

        [Column("CUSTOM_6")]
        [MaxLength(100)]
        public string? Custom6 { get; set; }

        [Column("ID_COMPANY")]
        public long CompanyId { get; set; }

        // Navigation properties
        public Company Company { get; set; } = null!;
        public ICollection<RefurbSale> RefurbSales { get; set; } = new List<RefurbSale>();

        // Validation
        public void SetPrice(double price)
        {
            if (price < 0)
                throw new ArgumentException("Price cannot be negative.");
            Price = price;
        }

        public void SetQty(int qty)
        {
            if (qty < 0)
                throw new ArgumentException("Qty cannot be negative.");
            Qty = qty;
        }
    }
}
