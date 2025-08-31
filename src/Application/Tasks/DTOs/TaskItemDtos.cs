namespace Application.Tasks.DTOs;

public record TaskItemDto(int Id, string Title, bool IsCompleted, int ProjectId);
public record CreateTaskItemDto(string Title);
public record UpdateTaskItemDto(string? Title, bool? IsCompleted);
public record ToggleTaskCompletionDto(bool IsCompleted);


