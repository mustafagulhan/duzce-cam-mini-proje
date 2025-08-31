namespace Application.Projects.DTOs;

public record ProjectDto(int Id, string Name, string? Description);
public record CreateProjectDto(string Name, string? Description);
public record UpdateProjectDto(string Name, string? Description);


