using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationPortal.Models
{
    [Table("Statuses")]
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Because SQL script doesn't use Identity here
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}