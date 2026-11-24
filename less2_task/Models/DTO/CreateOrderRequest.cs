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
    public int Quantity { get; set; }
    /// <summary>
    /// Price
    /// </summary>
    [Description("The price of the order")]
    public decimal Price { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    [Description("An optional comment for the order")]
    public string Comment { get; set; }

}
