using Microsoft.Identity.Client;

namespace To_Do_List_Web_API.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? CompletedAt { get; set; }

    }
}
