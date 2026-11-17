using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace less2_task.Models.DTO;

/// <summary>
/// Create new order request
/// </summary>
[Description("Request containing quantity, price, and optional comment for a new order")]
public class CreateOrderRequest
{
    /// <summary>
    /// Quantity of items
    /// </summary>
    [Description("The quantity of items to order")]
    [Range(1, int.MaxValue, ErrorMessage = "The quantity of items must be greater than zero")]
    public int Quantity { get; set; }
    /// <summary>
    /// Price
    /// </summary>
    [Description("The price of the order")]
    [Range(1.00, double.MaxValue, ErrorMessage = "The price must be greater than zero")]
    public decimal Price { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    [Description("An optional comment for the order")]
    [MaxLength(200, ErrorMessage = "Comments must not exceed 200 characters")]
    public string Comment { get; set; }

}
