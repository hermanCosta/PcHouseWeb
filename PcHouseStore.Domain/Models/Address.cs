using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("address")]
public class Address
{
    [Key]
    [Column("address_id")]
    public long AddressId { get; set; }

    [Required]
    [MaxLength(160)]
    [Column("line1")]
    public string Line1 { get; set; } = string.Empty;

    [MaxLength(160)]
    [Column("line2")]
    public string? Line2 { get; set; }

    [Required]
    [MaxLength(120)]
    [Column("city")]
    public string City { get; set; } = string.Empty;

    [MaxLength(120)]
    [Column("county")]
    public string? County { get; set; }

    [MaxLength(20)]
    [Column("postcode")]
    public string? Postcode { get; set; }

    [Required]
    [MaxLength(2)]
    [Column("country_iso2")]
    public string CountryIso2 { get; set; } = "IE";

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Company> BillingCompanies { get; set; } = new List<Company>();
    public ICollection<Company> ShippingCompanies { get; set; } = new List<Company>();
    public ICollection<PersonAddress> PersonAddresses { get; set; } = new List<PersonAddress>();
}
