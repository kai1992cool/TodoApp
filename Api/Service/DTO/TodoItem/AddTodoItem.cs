using System.ComponentModel.DataAnnotations;

namespace Service.DTO.TodoItem
{
    public class AddTodoItem
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

    }
}
