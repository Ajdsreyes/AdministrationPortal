using Microsoft.AspNetCore.Mvc;

namespace AdministrationPortal.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalUsers { get; set; }
        public int TotalRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int ReadyToClaim { get; set; }
    }
}
