using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("payment_method")]
public class PaymentMethod
{
    [Key]
    [Column("payment_method_id")]
    public long PaymentMethodId { get; set; }

    [Required]
    [Column("company_id")]
    public long CompanyId { get; set; }

    [Required]
    [Column("method_code")]
    public PayMethod MethodCode { get; set; } = PayMethod.Cash;

    [MaxLength(120)]
    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    public Company Company { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
