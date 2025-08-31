using Application.Projects;
using Application.Projects.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll(CancellationToken ct)
    {
        var items = await _projectService.GetAllAsync(ct);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _projectService.GetByIdAsync(id, ct);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto, CancellationToken ct)
    {
        var created = await _projectService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto, CancellationToken ct)
    {
        var ok = await _projectService.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _projectService.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}


