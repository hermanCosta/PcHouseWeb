using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcHouseStore.Domain.Models
{
    [Table("PERSON")]
    public class Person
    {
        [Key]
        [Column("ID_PERSON")]
        public long PersonId { get; set; }

        [Required]
        [Column("FIRST_NAME")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column("LAST_NAME")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("CONTACT_NO")]
        [MaxLength(30)]
        public string ContactNo { get; set; } = string.Empty;

        [Column("EMAIL")]
        [MaxLength(255)]
        public string? Email { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}