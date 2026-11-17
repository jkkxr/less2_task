using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace less2_task.Models.DTO;

/// <summary>
/// Login request
/// </summary>
[Description("Request model for user login")]
public class LoginRequest
{
    /// <summary>
    /// Email (as login)
    /// </summary>
    [Description("The email address of the user to be authenticated")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; }

    /// <summary>
    /// User's password
    /// </summary>
    [Description("The password for the user to be authenticated")]
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
}
