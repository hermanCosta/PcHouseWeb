using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("refurb_attribute")]
public class RefurbAttribute
{
    [Key]
    [Column("refurb_attribute_id")]
    public long RefurbAttributeId { get; set; }

    [Required]
    [Column("refurb_template_id")]
    public long RefurbTemplateId { get; set; }

    [Required]
    [MaxLength(80)]
    [Column("attribute_name")]
    public string AttributeName { get; set; } = string.Empty;

    [Required]
    [Column("data_type")]
    public AttributeDataType DataType { get; set; } = AttributeDataType.Text;

    [Required]
    [Column("is_required")]
    public bool IsRequired { get; set; }

    [Required]
    [Column("display_order")]
    public int DisplayOrder { get; set; }

    public RefurbTemplate RefurbTemplate { get; set; } = null!;
    public ICollection<RefurbAttributeValue> Values { get; set; } = new List<RefurbAttributeValue>();
}
