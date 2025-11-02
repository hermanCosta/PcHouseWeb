using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("refurb_attribute_value")]
public class RefurbAttributeValue
{
    [Key]
    [Column("refurb_attribute_value_id")]
    public long RefurbAttributeValueId { get; set; }

    [Required]
    [Column("refurb_item_id")]
    public long RefurbItemId { get; set; }

    [Required]
    [Column("refurb_attribute_id")]
    public long RefurbAttributeId { get; set; }

    [Column("value_text")]
    public string? ValueText { get; set; }

    [Column("value_number")]
    public decimal? ValueNumber { get; set; }

    [Column("value_boolean")]
    public bool? ValueBoolean { get; set; }

    [Column("value_date")]
    public DateTime? ValueDate { get; set; }

    public RefurbItem RefurbItem { get; set; } = null!;
    public RefurbAttribute RefurbAttribute { get; set; } = null!;
}
