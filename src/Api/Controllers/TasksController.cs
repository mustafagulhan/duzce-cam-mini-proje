using Application.Tasks;
using Application.Tasks.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskItemService _taskService;

    public TasksController(ITaskItemService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItemDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _taskService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemDto dto, CancellationToken ct)
    {
        var ok = await _taskService.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> ToggleComplete(int id, [FromBody] ToggleTaskCompletionDto dto, CancellationToken ct)
    {
        var ok = await _taskService.ToggleCompletionAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _taskService.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}


