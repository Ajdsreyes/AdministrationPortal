namespace AdministrationPortal.Models.ViewModels
{
    public class ServiceRequirementVM
    {
        public int PurposeId { get; set; }
        public int ServiceId { get; set; }

        public string PurposeName { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }

        public List<string> Options { get; set; } = new();

        // NEW
        public bool IsDeleted { get; set; }
    }

}