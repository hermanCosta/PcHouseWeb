using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PcHouseStore.Domain.Models;

[Table("company")]
public class Company
{
    [Key]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Required]
    [MaxLength(160)]
    [Column("legal_name")]
    public string LegalName { get; set; } = string.Empty;

    [MaxLength(160)]
    [Column("trading_name")]
    public string? TradingName { get; set; }

    [MaxLength(32)]
    [Column("vat_number")]
    public string? VatNumber { get; set; }

    [MaxLength(32)]
    [Column("registration_number")]
    public string? RegistrationNumber { get; set; }

    [MaxLength(255)]
    [Column("email")]
    public string? Email { get; set; }

    [MaxLength(45)]
    [Column("phone_primary")]
    public string? PhonePrimary { get; set; }

    [MaxLength(45)]
    [Column("phone_secondary")]
    public string? PhoneSecondary { get; set; }

    [MaxLength(255)]
    [Column("website")]
    public string? Website { get; set; }

    [Column("billing_address_id")]
    public long? BillingAddressId { get; set; }

    [Column("shipping_address_id")]
    public long? ShippingAddressId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Address? BillingAddress { get; set; }
    public Address? ShippingAddress { get; set; }

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<CatalogItem> CatalogItems { get; set; } = new List<CatalogItem>();
    public ICollection<PriceBook> PriceBooks { get; set; } = new List<PriceBook>();
    public ICollection<RefurbItem> RefurbItems { get; set; } = new List<RefurbItem>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<CashMovement> CashMovements { get; set; } = new List<CashMovement>();
    public ICollection<DailyClosing> DailyClosings { get; set; } = new List<DailyClosing>();

    [NotMapped]
    public string Name
    {
        get => TradingName ?? LegalName;
        set => TradingName = value;
    }

    [NotMapped]
    public string? ContactOne
    {
        get => PhonePrimary;
        set => PhonePrimary = value;
    }

    [NotMapped]
    public string? ContactTwo
    {
        get => PhoneSecondary;
        set => PhoneSecondary = value;
    }

    [NotMapped]
    public string? Address
    {
        get
        {
            if (BillingAddress is null)
            {
                return null;
            }

            var parts = new[] { BillingAddress.Line1, BillingAddress.Line2, BillingAddress.City, BillingAddress.County, BillingAddress.Postcode };
            return string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
        }
        set { /* compatibility shim - ignored */ }
    }

    [NotMapped]
    public string? Password { get; set; }
}