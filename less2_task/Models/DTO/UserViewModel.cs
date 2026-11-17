using System.ComponentModel;

namespace less2_task.Models.DTO;

/// <summary>
/// User's infortmation
/// </summary>
[Description("View model representing user public information")]
public class UserViewModel
{
    /// <summary>
    /// User's name
    /// </summary>
    [Description("The name of the user")]
    public string Name {get; set;}
    /// <summary>
    /// User's email
    /// </summary>
    [Description("The email address of the user")]
    public string Email {get; set;}
}
