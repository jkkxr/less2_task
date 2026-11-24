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
    public string Email { get; set; }

    /// <summary>
    /// User's password
    /// </summary>
    [Description("The password for the user to be authenticated")]
    public string Password { get; set; }
}
