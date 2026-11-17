using System.ComponentModel.DataAnnotations;

namespace less2_task.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
