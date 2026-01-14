namespace AdministrationPortal.Models.ViewModels
{
    public class ServiceRequirementVM
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }

        public string Label { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }

        public List<string> Options { get; set; } = new();

        // NEW
        public bool IsDeleted { get; set; }
    }

}