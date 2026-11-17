using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace less2_task.Models.DTO;

/// <summary>
/// Registration
/// </summary>
[Description("Request model for user registration")]
public class RegisterRequest
{
    /// <summary>
    /// User's name
    /// </summary>
    [Description("The name of the user to be registered")]
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }

    /// <summary>
    /// User's email address
    /// </summary>
    [Description("The email address of the user to be registered")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; }

    /// <summary>
    /// User's password
    /// </summary>
    [Description("The password for the user to be registered")]
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
}
