using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _dbContext;

    public TaskItemRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TaskItems.Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<List<TaskItem>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TaskItems.AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskItem> AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _dbContext.TaskItems.Add(taskItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return taskItem;
    }

    public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _dbContext.TaskItems.Update(taskItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _dbContext.TaskItems.Remove(taskItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}


