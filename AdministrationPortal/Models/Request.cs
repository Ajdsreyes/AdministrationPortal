using Microsoft.AspNetCore.Mvc;

namespace AdministrationPortal.Models
{
    public class Request
    {
        public int Id { get; set; }

        public string ServiceType { get; set; } // Barangay Clearance, Cedula, etc.
        public string Status { get; set; }      // Pending, Approved, Ready
    }

}
