namespace AdministrationPortal.Models.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }

        // Name
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }

        // Personal
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string CivilStatus { get; set; }
        public string Religion { get; set; }

        // Address
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int StayYears { get; set; }
        public int StayMonths { get; set; }

        // Contact
        public string ContactNumber { get; set; }
        public string Email { get; set; }

        // Status
        public bool IsVoter { get; set; }
        public bool IsActive { get; set; }

        public string PhotoUrl { get; set; }
    

    // ==========================
// DISPLAY-ONLY (FOR LIST)
// ==========================

public string FullName =>
    $"{LastName}, {FirstName} {MiddleName} {Suffix}".Trim();

        public string Address =>
            $"{Street}, {Barangay}, {City}";
    }
}
