using Microsoft.AspNetCore.Mvc;
using docker_lab_backend.Models;
using docker_lab_backend.Services;

namespace docker_lab_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetAll()
    {
        return Ok(_taskService.GetAllTasks());
    }

    [HttpGet("{id}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = _taskService.GetTaskById(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskItem> Create(TaskItem task)
    {
        var createdTask = _taskService.CreateTask(task);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskItem task)
    {
        if (id != task.Id)
            return BadRequest("Task ID mismatch.");

        var updated = _taskService.UpdateTask(id, task);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _taskService.DeleteTask(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
