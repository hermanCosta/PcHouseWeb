using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("person")]
public class Person
{
    [Key]
    [Column("person_id")]
    public long PersonId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("preferred_name")]
    public string? PreferredName { get; set; }

    [MaxLength(255)]
    [Column("email")]
    public string? Email { get; set; }

    [MaxLength(30)]
    [Column("phone_mobile")]
    public string? PhoneMobile { get; set; }

    [MaxLength(30)]
    [Column("phone_home")]
    public string? PhoneHome { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<PersonAddress> Addresses { get; set; } = new List<PersonAddress>();

    public string DisplayName => string.IsNullOrWhiteSpace(PreferredName)
        ? $"{FirstName} {LastName}"
        : PreferredName!;

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [NotMapped]
    public string? ContactNo
    {
        get => PhoneMobile;
        set => PhoneMobile = value;
    }
}