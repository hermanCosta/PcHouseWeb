using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("catalog_item")]
public class CatalogItem
{
    [Key]
    [Column("catalog_item_id")]
    public long CatalogItemId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [MaxLength(60)]
    [Column("sku")]
    public string? Sku { get; set; }

    [Required]
    [MaxLength(160)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("item_type")]
    public CatalogItemType ItemType { get; set; } = CatalogItemType.Product;

    [Required]
    [MaxLength(20)]
    [Column("default_uom")]
    public string DefaultUom { get; set; } = "EA";

    [Required]
    [Column("track_inventory")]
    public bool TrackInventory { get; set; } = true;

    [Required]
    [Column("taxable")]
    public bool Taxable { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Company Company { get; set; } = null!;
    public RefurbTemplate? RefurbTemplate { get; set; }
    public ICollection<PriceBookItem> PriceBookItems { get; set; } = new List<PriceBookItem>();
    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
