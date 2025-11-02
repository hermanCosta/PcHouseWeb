using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("SERVICE_ORDER_PROD_SERV")]
    public class ServiceOrderProdServ
    {
        [Key]
        [Column("ID_SERVICE_ORDER_PROD_SERV")]
        public long ServiceOrderProdServId { get; set; }

        [Column("ID_SERVICE_ORDER")]
        public long ServiceOrderId { get; set; }

        [Column("ID_PROD_SERV")]
        public long ProdServId { get; set; }

        [Column("QTY")]
        public int Qty { get; set; }

        [Column("TOTAL")]
        public double? Total { get; set; }

        // Navigation properties
        public ServiceOrder ServiceOrder { get; set; } = null!;
        public ProductService ProductService { get; set; } = null!;

        // Validation
        public void SetQty(int qty)
        {
            if (qty <= 0)
                throw new ArgumentException("Qty must be greater than 0.");
            Qty = qty;
        }

        public void SetTotal(double? total)
        {
            if (total.HasValue && total < 0)
                throw new ArgumentException("Total cannot be negative.");
            Total = total;
        }
    }
}
