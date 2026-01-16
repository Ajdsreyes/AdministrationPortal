using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationPortal.Models
{
    [Table("Histories")] // Matches the table name in your OnModelCreating
    public class History
    {
        [Key]
        public int HistoryId { get; set; }

        public int? RequestId { get; set; }

        public string Action { get; set; } = string.Empty; // e.g., "Status Updated", "Profile Edited"

        public string PerformedBy { get; set; } = string.Empty; // Admin Username

        public DateTime LogDate { get; set; } = DateTime.Now;

        public string? Details { get; set; }

        // Optional: Navigation property to link to the request
        [ForeignKey("RequestId")]
        public virtual ServiceRequest? ServiceRequest { get; set; }
    }
}