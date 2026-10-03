using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public sealed class RegistrationForm
{
    [Required(ErrorMessage = "Enter your name.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Use 2 to 80 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter your email address.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
}