using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public enum Priority
    {
        Low,
        Medium,
        High
    }
    public enum Status
    {
        Pending,
        InProgress,
        Completed,
        Archived
    }

    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        [Required]
        public Status Status { get; set; } = Status.Pending;
    }
}