using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationPortal.Models
{
    [Table("ServicePurposes")]
    public class ServicePurpose
    {
        [Key]
        public int PurposeId { get; set; }

        public int ServiceId { get; set; }

        // This maps to "Label" in your ViewModel
        public string PurposeName { get; set; } = string.Empty;

        // NEW: text, number, date, select
        public string? FieldType { get; set; }

        public bool IsRequired { get; set; }

        // Stored as a comma-separated string in DB (e.g., "Option1,Option2")
        public string? Options { get; set; }

        public bool IsEnabled { get; set; }
    }
}