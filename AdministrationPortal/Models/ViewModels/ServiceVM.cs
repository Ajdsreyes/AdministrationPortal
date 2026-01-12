namespace AdministrationPortal.Models.ViewModels
{
    public class ServiceVM
    {
        // ===== BASIC SERVICE INFO =====
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get; set; }

        // ===== SERVICE ANALYTICS =====
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Ready { get; set; }
        public int Rejected { get; set; }

        // Optional (nice to have)
        public int TotalRequests =>
            Pending + Approved + Ready + Rejected;
    }
}
