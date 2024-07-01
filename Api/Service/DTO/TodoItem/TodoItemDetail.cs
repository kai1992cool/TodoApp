﻿namespace Service.DTO.TodoItem
{
    public class TodoItemDetail
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
