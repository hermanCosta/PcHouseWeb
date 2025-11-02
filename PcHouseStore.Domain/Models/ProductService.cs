using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("PROD_SERV")]
    public class ProductService
    {
        [Key]
        [Column("ID_PROD_SERV")]
        public long ProductServiceId { get; set; }

        [Required]
        [Column("NAME")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("QTY")]
        public int Quantity { get; set; } = 1;

        [Column("PRICE")]
        public double? Price { get; set; }

        [Column("MIN_QTY")]
        public int MinQuantity { get; set; } = 1;

        [Column("CATEGORY")]
        [MaxLength(45)]
        public string? Category { get; set; }

        [Column("NOTE")]
        [MaxLength(255)]
        public string? Note { get; set; }

        [Column("ID_COMPANY")]
        public long CompanyId { get; set; }

        // Navigation properties
        public Company Company { get; set; } = null!;
        public ICollection<SaleProdServ> SaleProdServs { get; set; } = new List<SaleProdServ>();
        public ICollection<ServiceOrderProdServ> ServiceOrderProdServs { get; set; } = new List<ServiceOrderProdServ>();

        // Validation
        public void SetQty(int qty)
        {
            if (qty < 0)
                throw new ArgumentException("Qty cannot be negative.");
            Quantity = qty;
        }

        public void SetPrice(double? price)
        {
            if (price.HasValue && price < 0)
                throw new ArgumentException("Price cannot be negative.");
            Price = price;
        }

        public void SetMinQty(int minQty)
        {
            if (minQty < 0)
                throw new ArgumentException("Minimum qty cannot be negative.");
            MinQuantity = minQty;
        }

        public bool IsLowStock => Quantity <= MinQuantity;
    }
}