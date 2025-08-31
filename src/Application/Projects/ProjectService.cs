using Application.Common.Interfaces;
using Application.Projects.DTOs;
using Domain.Entities;

namespace Application.Projects;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync(CancellationToken ct = default);
    Task<ProjectDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProjectDto> CreateAsync(CreateProjectDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateProjectDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projects;

    public ProjectService(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<List<ProjectDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _projects.GetAllAsync(ct);
        return list.Select(p => new ProjectDto(p.Id, p.Name, p.Description)).ToList();
    }

    public async Task<ProjectDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _projects.GetByIdAsync(id, ct);
        return entity is null ? null : new ProjectDto(entity.Id, entity.Name, entity.Description);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name is required");

        var entity = new Project
        {
            Name = dto.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description!.Trim()
        };
        var created = await _projects.AddAsync(entity, ct);
        return new ProjectDto(created.Id, created.Name, created.Description);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto, CancellationToken ct = default)
    {
        var entity = await _projects.GetByIdAsync(id, ct);
        if (entity is null) return false;

        if (!string.IsNullOrWhiteSpace(dto.Name)) entity.Name = dto.Name.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description!.Trim();
        await _projects.UpdateAsync(entity, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _projects.GetByIdAsync(id, ct);
        if (entity is null) return false;
        await _projects.DeleteAsync(entity, ct);
        return true;
    }
}


