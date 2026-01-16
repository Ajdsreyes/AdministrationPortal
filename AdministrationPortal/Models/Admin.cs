using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationPortal.Models
{
    [Table("Admins")] // Matches your groupmate's SQL table name
    public class Admin
    {
        [Key]
        public int AdminId { get; set; } // Primary Key

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string AdminPassword { get; set; } = string.Empty; // Matches SQL column name

        public DateTime? CreatedAt { get; set; }
    }
}