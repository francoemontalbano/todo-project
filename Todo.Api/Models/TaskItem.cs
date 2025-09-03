﻿namespace Todo.Api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
