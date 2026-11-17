using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace less2_task.Models.DTO;

/// <summary>
/// Request to update order's quantity
/// </summary>
[Description("Request containing the new quantity for the order")]
public class UpdateOrderQuantityRequest
{
    /// <summary>
    /// New quantity of items
    /// </summary>
    [Description("The updated quantity of items")]
    [Range(1, int.MaxValue, ErrorMessage = "The quantity of items must be greater than zero")]
    public int Quantity { get; set; }
}
