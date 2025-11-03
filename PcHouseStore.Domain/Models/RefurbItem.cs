using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("refurb_item")]
public class RefurbItem
{
    [Key]
    [Column("refurb_item_id")]
    public long RefurbItemId { get; set; }

    [Required]
    [Column("refurb_template_id")]
    public long RefurbTemplateId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [MaxLength(120)]
    [Column("serial_number")]
    public string? SerialNumber { get; set; }

    [Required]
    [Column("condition_grade")]
    public RefurbCondition ConditionGrade { get; set; } = RefurbCondition.B;

    [Precision(12, 2)]
    [Column("purchase_cost")]
    public decimal? PurchaseCost { get; set; }

    [Required]
    [Precision(12, 2)]
    [Column("list_price")]
    public decimal ListPrice { get; set; }

    [Required]
    [Column("quantity_on_hand")]
    public int QuantityOnHand { get; set; } = 1;

    [Required]
    [Column("status")]
    public RefurbInventoryStatus Status { get; set; } = RefurbInventoryStatus.InStock;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public RefurbTemplate RefurbTemplate { get; set; } = null!;
    public Company Company { get; set; } = null!;
    public ICollection<RefurbAttributeValue> AttributeValues { get; set; } = new List<RefurbAttributeValue>();
    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
