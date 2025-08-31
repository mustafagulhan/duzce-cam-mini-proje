using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITaskItemRepository
{
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<TaskItem>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
    Task<TaskItem> AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
}


