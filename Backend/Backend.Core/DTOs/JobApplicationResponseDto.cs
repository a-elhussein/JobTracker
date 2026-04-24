namespace Backend.Core.DTOs;

public class JobApplicationResponseDto
{
    public Guid Id { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DateApplied { get; set; }
    public string? Notes { get; set; }
    public string? SalaryRange { get; set; }
    public string? Source { get; set; }
}