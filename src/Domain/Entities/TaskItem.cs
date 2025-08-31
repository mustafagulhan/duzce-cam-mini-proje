using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public int ProjectId { get; set; }

    public Project? Project { get; set; }
}


