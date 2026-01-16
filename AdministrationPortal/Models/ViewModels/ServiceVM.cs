namespace AdministrationPortal.Models.ViewModels
{
    public class ServiceVM
    {
        // Matches SQL ServiceId
        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        // Note: This isn't in the DB yet, but good for UI
        public string? Icon { get; set; }

        public bool IsEnabled { get; set; }

        // Analytics (Calculated in the Controller)
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Ready { get; set; }

        public int TotalRequests => Pending + Approved + Ready;
    }
}