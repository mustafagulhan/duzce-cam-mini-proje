using Application.Common.Interfaces;
using Application.Tasks.DTOs;
using Domain.Entities;

namespace Application.Tasks;

public interface ITaskItemService
{
    Task<List<TaskItemDto>> GetByProjectIdAsync(int projectId, CancellationToken ct = default);
    Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TaskItemDto?> CreateAsync(int projectId, CreateTaskItemDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateTaskItemDto dto, CancellationToken ct = default);
    Task<bool> ToggleCompletionAsync(int id, ToggleTaskCompletionDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _tasks;
    private readonly IProjectRepository _projects;

    public TaskItemService(ITaskItemRepository tasks, IProjectRepository projects)
    {
        _tasks = tasks;
        _projects = projects;
    }

    public async Task<List<TaskItemDto>> GetByProjectIdAsync(int projectId, CancellationToken ct = default)
    {
        var list = await _tasks.GetByProjectIdAsync(projectId, ct);
        return list.Select(t => new TaskItemDto(t.Id, t.Title, t.IsCompleted, t.ProjectId)).ToList();
    }

    public async Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _tasks.GetByIdAsync(id, ct);
        return entity is null ? null : new TaskItemDto(entity.Id, entity.Title, entity.IsCompleted, entity.ProjectId);
    }

    public async Task<TaskItemDto?> CreateAsync(int projectId, CreateTaskItemDto dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) throw new ArgumentException("Title is required");
        var exists = await _projects.ExistsAsync(projectId, ct);
        if (!exists) return null;

        var entity = new TaskItem { Title = dto.Title.Trim(), ProjectId = projectId, IsCompleted = false };
        var created = await _tasks.AddAsync(entity, ct);
        return new TaskItemDto(created.Id, created.Title, created.IsCompleted, created.ProjectId);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskItemDto dto, CancellationToken ct = default)
    {
        var entity = await _tasks.GetByIdAsync(id, ct);
        if (entity is null) return false;
        if (!string.IsNullOrWhiteSpace(dto.Title)) entity.Title = dto.Title!.Trim();
        if (dto.IsCompleted.HasValue) entity.IsCompleted = dto.IsCompleted.Value;
        await _tasks.UpdateAsync(entity, ct);
        return true;
    }

    public async Task<bool> ToggleCompletionAsync(int id, ToggleTaskCompletionDto dto, CancellationToken ct = default)
    {
        var entity = await _tasks.GetByIdAsync(id, ct);
        if (entity is null) return false;
        entity.IsCompleted = dto.IsCompleted;
        await _tasks.UpdateAsync(entity, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _tasks.GetByIdAsync(id, ct);
        if (entity is null) return false;
        await _tasks.DeleteAsync(entity, ct);
        return true;
    }
}


