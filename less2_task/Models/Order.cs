using System.ComponentModel.DataAnnotations;

namespace less2_task.Models;

public class Order
{
    public int Id { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The quantity of items must be greater than zero")]
    public int Quantity { get; set; }
    [Range(1.00, double.MaxValue, ErrorMessage = "The price must be greater than zero")]
    public decimal Price { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime OrderDate { get; set; } = DateTime.Now;
    [MaxLength(200, ErrorMessage = "Comments must not exceed 200 characters")]
    public string Comment { get; set; }
}
