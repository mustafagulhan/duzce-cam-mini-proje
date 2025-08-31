using Application.Tasks;
using Application.Tasks.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/projects/{projectId:int}/tasks")]
public class ProjectTasksController : ControllerBase
{
    private readonly ITaskItemService _taskService;

    public ProjectTasksController(ITaskItemService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskItemDto>>> GetByProject(int projectId, CancellationToken ct)
    {
        var items = await _taskService.GetByProjectIdAsync(projectId, ct);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create(int projectId, [FromBody] CreateTaskItemDto dto, CancellationToken ct)
    {
        var created = await _taskService.CreateAsync(projectId, dto, ct);
        if (created is null) return NotFound();
        return CreatedAtAction("GetById", "Tasks", new { id = created.Id }, created);
    }
}


