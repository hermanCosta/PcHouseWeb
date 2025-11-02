using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("order_line")]
public class OrderLine
{
    [Key]
    [Column("order_line_id")]
    public long OrderLineId { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("catalog_item_id")]
    public long CatalogItemId { get; set; }

    [Column("refurb_item_id")]
    public long? RefurbItemId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Precision(12, 3)]
    [Column("quantity")]
    public decimal Quantity { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Precision(5, 2)]
    [Column("vat_rate")]
    public decimal VatRate { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("vat_amount")]
    public decimal VatAmount { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("line_total")]
    public decimal LineTotal { get; set; }

    [Required]
    [Column("fulfilment_status")]
    public OrderFulfilmentStatus FulfilmentStatus { get; set; } = OrderFulfilmentStatus.Pending;

    public Order Order { get; set; } = null!;
    public CatalogItem CatalogItem { get; set; } = null!;
    public RefurbItem? RefurbItem { get; set; }
}
