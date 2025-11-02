using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models;

[Table("order_note")]
public class OrderNote
{
    [Key]
    [Column("order_note_id")]
    public long OrderNoteId { get; set; }

    [Required]
    [Column("order_id")]
    public long OrderId { get; set; }

    [Required]
    [Column("employee_id")]
    public long EmployeeId { get; set; }

    [Required]
    [MaxLength(1000)]
    [Column("note")]
    public string Note { get; set; } = string.Empty;

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Order Order { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
}
