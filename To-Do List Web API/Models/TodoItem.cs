using System.ComponentModel.DataAnnotations;
using To_Do_List_Web_API.Attributes;

namespace To_Do_List_Web_API.Models
{
    [CompletedStateValidation]
    [CompletedCreationDatesValidation]
    public class TodoItem
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? CompletedAt { get; set; }

    }
}
