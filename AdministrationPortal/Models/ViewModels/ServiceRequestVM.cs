namespace AdministrationPortal.Models.ViewModels
{
    public class ServiceRequestVM
    {
        // ===== REQUEST INFO =====
        public int RequestId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;

        // ===== RESIDENT BASIC INFO =====
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Suffix { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Sex { get; set; } = string.Empty;
        public string CivilStatus { get; set; } = string.Empty;
        public string Religion { get; set; } = string.Empty; // Added to match Controller mapping

        // ===== ADDRESS & CONTACT =====
        public string HouseNoStreet { get; set; } = string.Empty;
        public string Barangay { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Length of Stay (Matches Users Table) - FIXED CS1061
        public int StayYears { get; set; }
        public int StayMonths { get; set; }

        // ===== SERVICE-SPECIFIC =====
        public string? Purpose { get; set; } // Added for display
        public decimal? GrossAnnualIncome { get; set; }
        public string? UploadPath { get; set; }
        public DateTime? DateToClaim { get; set; }
        public TimeSpan? TimeToClaim { get; set; }

        // ===== DISPLAY HELPERS =====
        public string FullName => $"{LastName}, {FirstName} {MiddleName} {Suffix}".Trim();

        public int Age => DateOfBirth.HasValue
            ? DateTime.Today.Year - DateOfBirth.Value.Year
            : 0;
    }
}