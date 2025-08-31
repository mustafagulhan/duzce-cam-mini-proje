using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _dbContext;

    public ProjectRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects.Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return project;
    }

    public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
    {
        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects.AnyAsync(p => p.Id == id, cancellationToken);
    }
}


