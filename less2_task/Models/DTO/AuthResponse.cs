using System.ComponentModel;
namespace less2_task.Models.DTO;

/// <summary>
/// Returns after successful registration or authentication
/// </summary>
[Description("Response contains user's name and token")]
public class AuthResponse
{
    /// <summary>
    /// User's name
    /// </summary>
    [Description("The name of authenticated user")]
    public string Name { get; set; }

    /// <summary>
    /// Authentication token
    /// </summary>
    [Description("The access token for future logins")]
    public string Token { get; set; }
}