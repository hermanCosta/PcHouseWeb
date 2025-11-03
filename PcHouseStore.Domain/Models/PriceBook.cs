using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("price_book")]
public class PriceBook
{
    [Key]
    [Column("price_book_id")]
    public long PriceBookId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Required]
    [MaxLength(120)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    [Column("currency")]
    public string Currency { get; set; } = "EUR";

    [Required]
    [Column("valid_from")]
    public DateTime ValidFrom { get; set; }

    [Column("valid_to")]
    public DateTime? ValidTo { get; set; }

    [Required]
    [Column("is_default")]
    public bool IsDefault { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Company Company { get; set; } = null!;
    public ICollection<PriceBookItem> Items { get; set; } = new List<PriceBookItem>();
}
