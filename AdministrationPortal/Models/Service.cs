using System.ComponentModel.DataAnnotations;

public class Service
{
    [Key]
    public int ServiceId { get; set; } // Must match the Request's ServiceId
    public string ServiceName { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}