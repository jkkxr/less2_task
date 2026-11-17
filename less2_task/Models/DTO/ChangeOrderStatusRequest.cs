using System.ComponentModel;

namespace less2_task.Models.DTO;

/// <summary>
/// Order status change request
/// </summary>
[Description("Request to change the completion status of an order")]
public class ChangeOrderStatusRequest
{
    /// <summary>
    /// Completion status
    /// </summary>
    [Description("Indicates whether the order is completed or not")]
    public bool IsCompleted { get; set; } = false;
}
