using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("refurb_template")]
public class RefurbTemplate
{
    [Key]
    [Column("refurb_template_id")]
    public long RefurbTemplateId { get; set; }

    [Required]
    [Column("catalog_item_id")]
    public long CatalogItemId { get; set; }

    [MaxLength(100)]
    [Column("default_brand")]
    public string? DefaultBrand { get; set; }

    [MaxLength(150)]
    [Column("default_model")]
    public string? DefaultModel { get; set; }

    [Column("base_description")]
    public string? BaseDescription { get; set; }

    public CatalogItem CatalogItem { get; set; } = null!;
    public ICollection<RefurbAttribute> Attributes { get; set; } = new List<RefurbAttribute>();
    public ICollection<RefurbItem> RefurbItems { get; set; } = new List<RefurbItem>();
}
