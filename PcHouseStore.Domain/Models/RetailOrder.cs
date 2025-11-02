using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("retail_order")]
public class RetailOrder : Order
{
    public RetailOrder()
    {
        OrderType = OrderType.Retail;
    }

    [Required]
    [Column("sales_channel")]
    public SalesChannel SalesChannel { get; set; } = SalesChannel.InStore;
}
