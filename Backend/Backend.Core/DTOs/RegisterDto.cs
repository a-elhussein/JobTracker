using System.ComponentModel.DataAnnotations;

namespace Backend.Core.DTOs;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(6)]
    public string Password { get; set; } = string.Empty;
}