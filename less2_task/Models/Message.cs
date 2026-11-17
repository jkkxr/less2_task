using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace less2_task.Models
{
    public class Message
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required (ErrorMessage = "The 'Sender' field is required.")]
        [MinLength(5, ErrorMessage = "The 'Sender' field must be at least 5 characters long.")]
        [MaxLength(50, ErrorMessage = "The 'Sender' field must be at most 50 characters long.")]
        [EmailAddress(ErrorMessage = "The 'Sender' field must be a valid email address.")]
        public string Sender { get; set; }

        [Required(ErrorMessage = "The 'Text' field is required.")]
        [MinLength(3, ErrorMessage = "The 'Text' field must be at least 3 characters long.")]
        [MaxLength(500, ErrorMessage = "The 'Text' field must be at most 500 characters long.")]
        public string Text { get; set; }

        [BindNever]
        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
