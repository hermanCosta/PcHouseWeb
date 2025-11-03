using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("product_service")]
public class ProductService
{
    [Key]
    [Column("product_service_id")]
    public long ProductServiceId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Required]
    [MaxLength(160)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("category")]
    public string? Category { get; set; }

    [Required]
    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("min_quantity")]
    public int MinQuantity { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public Company Company { get; set; } = null!;

    // Computed property
    [NotMapped]
    public bool IsLowStock => Quantity <= MinQuantity;
}

