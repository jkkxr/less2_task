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
    public string Name { get; set; }

    /// <summary>
    /// User's email address
    /// </summary>
    [Description("The email address of the user to be registered")]
    public string Email { get; set; }

    /// <summary>
    /// User's password
    /// </summary>
    [Description("The password for the user to be registered")]
    public string Password { get; set; }
}
