using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("SALE_PROD_SERV")]
    public class SaleProdServ
    {
        [Key]
        [Column("ID_SALE_PROD_SERV")]
        public long SaleProdServId { get; set; }

        [Column("ID_SALE")]
        public long SaleId { get; set; }

        [Column("ID_PROD_SERV")]
        public long ProdServId { get; set; }

        [Column("QTY")]
        public int Qty { get; set; }

        [Column("TOTAL")]
        public double Total { get; set; }

        // Navigation properties
        public Sale Sale { get; set; } = null!;
        public ProductService ProductService { get; set; } = null!;

        // Validation
        public void SetQty(int qty)
        {
            if (qty <= 0)
                throw new ArgumentException("Qty must be greater than 0.");
            Qty = qty;
        }

        public void SetTotal(double total)
        {
            if (total < 0)
                throw new ArgumentException("Total cannot be negative.");
            Total = total;
        }
    }
}
