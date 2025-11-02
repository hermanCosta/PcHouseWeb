using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PcHouseStore.Domain.Models;

[Table("price_book_item")]
public class PriceBookItem
{
    [Key]
    [Column("price_book_item_id")]
    public long PriceBookItemId { get; set; }

    [Required]
    [Column("price_book_id")]
    public long PriceBookId { get; set; }

    [Required]
    [Column("catalog_item_id")]
    public long CatalogItemId { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Precision(5, 2)]
    [Column("vat_rate")]
    public decimal VatRate { get; set; }

    [Required]
    [Column("min_qty")]
    public int MinQty { get; set; } = 1;

    [Column("max_qty")]
    public int? MaxQty { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public PriceBook PriceBook { get; set; } = null!;
    public CatalogItem CatalogItem { get; set; } = null!;
}
