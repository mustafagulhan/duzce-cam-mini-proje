using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default);
    Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
    Task DeleteAsync(Project project, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}


