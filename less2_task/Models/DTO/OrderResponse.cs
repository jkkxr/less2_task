using System.ComponentModel;

namespace less2_task.Models.DTO;

/// <summary>
/// Order response
/// </summary>
[Description("Response representing order details including text status")]
public class OrderResponse
{
    /// <summary>
    /// Unique id
    /// </summary>
    [Description("The unique id of the order")]
    public int Id { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [Description("Quantity of items")]
    public int Quantity { get; set; }
    /// <summary>
    /// Price
    /// </summary>
    [Description("The price of the order")]
    public decimal Price { get; set; }
    /// <summary>
    /// Completion
    /// </summary>
    [Description("Boolean flag indicating completion")]
    public bool IsCompleted { get; set; } = false;
    /// <summary>
    /// Date and time of the order
    /// </summary>
    [Description("The timestamp when the order was created")]
    public DateTime OrderDate { get; set; } = DateTime.Now;
    /// <summary>
    /// Status of the order
    /// </summary>
    [Description("Status ('completed' or 'not completed')")]
    public string Status { get; set; }
    /// <summary>
    /// Order comment
    /// </summary>
    [Description("The comment associated with the order")]
    public string Comment { get; set; }
}
