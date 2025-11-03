using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PcHouseStore.Domain.Models;

[Table("person_address")]
public class PersonAddress
{
    [Column("person_id", Order = 0)]
    public long PersonId { get; set; }

    [Column("address_id", Order = 1)]
    public long AddressId { get; set; }

    [Column("usage_type", Order = 2)]
    [MaxLength(16)]
    public string UsageType { get; set; } = "HOME";

    public Person Person { get; set; } = null!;
    public Address Address { get; set; } = null!;
}
