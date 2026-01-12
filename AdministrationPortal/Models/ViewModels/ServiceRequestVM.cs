namespace AdministrationPortal.Models.ViewModels
{
    public class ServiceRequestVM
    {
        // ===== REQUEST INFO =====
        public int Id { get; set; }
        public string ServiceType { get; set; }
        public DateTime DateRequested { get; set; }
        public string Status { get; set; }

        // ===== RESIDENT BASIC INFO =====
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string CivilStatus { get; set; }
        public string Religion { get; set; }

        // ===== ADDRESS =====
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public int StayYears { get; set; }
        public int StayMonths { get; set; }

        // ===== CONTACT =====
        public string ContactNumber { get; set; }
        public string Email { get; set; }

        // ===== SERVICE-SPECIFIC =====
        public string Purpose { get; set; } // Clearance / Indigency / etc
        public decimal? GrossAnnualIncome { get; set; } // Cedula only
        public string UploadedPhotoUrl { get; set; } // Barangay ID

        public DateTime? DateToClaim { get; set; }
        public TimeSpan? TimeToClaim { get; set; }

        // ===== DISPLAY HELPERS =====
        public string FullName =>
            $"{LastName}, {FirstName} {MiddleName} {Suffix}".Trim();
    }
}
