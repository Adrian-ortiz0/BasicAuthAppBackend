using BasicAuthApplication.Dtos;
using BasicAuthApplication.Interfaces;
using BasicAuthDomain.Entities;
using BasicAuthDomain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private int GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
        {
            throw new Exception("Usuario no autenticado");
        }
        return int.Parse(userIdClaim);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAllTasks()
    {
        try
        {
            var userId = GetAuthenticatedUserId();
            var tasks = await _taskService.GetAllTasksAsync(userId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponseDto>> GetTaskById(int id)
    {
        try
        {
            var userId = GetAuthenticatedUserId();
            var task = await _taskService.GetTaskByIdAsync(id, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] CreateTaskDto request)
    {
        try
        {
            var userId = GetAuthenticatedUserId();
            var task = await _taskService.CreateTaskAsync(request, userId);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponseDto>> UpdateTask(int id, [FromBody] UpdateTaskDto request)
    {
        try
        {
            var userId = GetAuthenticatedUserId();
            var task = await _taskService.UpdateTaskAsync(id, request, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        try
        {
            var userId = GetAuthenticatedUserId();
            await _taskService.DeleteTaskAsync(id, userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}