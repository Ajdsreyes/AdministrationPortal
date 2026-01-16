using AdministrationPortal.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace AdministrationPortal.Models
{
    [Table("ServiceRequests")] // Maps to the SQL table name
    public class ServiceRequest
    {
        [Key]
        public int RequestId { get; set; } // Primary Key (Matches SQL)

        // Foreign Keys
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int PurposeId { get; set; }
        public int StatusId { get; set; }

        // Form Data
        public decimal? GrossAnnualIncome { get; set; }
        public string? UploadPath { get; set; }

        // Schedule & Tracking
        [DataType(DataType.Date)]
        public DateTime? DateToClaim { get; set; }
        public TimeSpan? TimeToClaim { get; set; }
        public DateTime? CreatedAt { get; set; }

        // ==========================================
        // NAVIGATION PROPERTIES
        // These allow you to access linked data like request.User.FirstName
        // ==========================================
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status? Status { get; set; }
    }
}