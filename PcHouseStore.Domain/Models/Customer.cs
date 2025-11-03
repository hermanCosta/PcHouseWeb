using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("customer")]
public class Customer
{
    [Key]
    [Column("customer_id")]
    public long CustomerId { get; set; }

    [Required]
    [Column("person_id")]
    public long PersonId { get; set; }

    [Column("company_id")]
    public long? CompanyId { get; set; }

    [Required]
    [Column("marketing_opt_in")]
    public bool MarketingOptIn { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Person Person { get; set; } = null!;
    public Company? Company { get; set; }
    public ICollection<Device> Devices { get; set; } = new List<Device>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}