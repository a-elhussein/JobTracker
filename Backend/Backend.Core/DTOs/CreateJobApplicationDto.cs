using System.ComponentModel.DataAnnotations;

namespace Backend.Core.DTOs;

public class CreateJobApplicationDto
{
    [Required] [MaxLength(100)] 
    public string Company { get; set; } = string.Empty;
    [Required] [MaxLength(100)] 
    public string Role { get; set; } = string.Empty;
    [Required] [MaxLength(50)] 
    public string Status { get; set; } = "Applied";
    public DateTime DateApplied { get; set; }
    [MaxLength(1000)] 
    public string? Notes { get; set; }
    [MaxLength(50)] 
    public string? SalaryRange { get; set; }
    [MaxLength(50)] 
    public string? Source { get; set; }
}