using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationPortal.Models
{
    [Table("Users")] // Maps to your SQL table name
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string? Suffix { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Sex { get; set; } = string.Empty;
        public string CivilStatus { get; set; } = string.Empty;
        public string Religion { get; set; } = string.Empty;

        public string HouseNoStreet { get; set; } = string.Empty;
        public string Barangay { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;

        public int StayYears { get; set; }
        public int StayMonths { get; set; }

        public string? ContactNo { get; set; }
        public string Email { get; set; } = string.Empty;

        public string? UploadPath { get; set; } // The actual database column for the photo

        public bool IsVoter { get; set; }
        public bool IsActive { get; set; }
    }
}