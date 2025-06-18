using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using TMS.API.Exceptions;

namespace TaskManagerAPI.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> GetAll(
             [FromQuery] string? priority,
             [FromQuery] string? status,
             CancellationToken cancellationToken,
             [FromQuery] int skip = 0,
             [FromQuery] int take = 10 
             )
        {
            Priority? parsedPriority = null;
            Status? parsedStatus = null;

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<Priority>(priority, true, out var p))
                parsedPriority = p;

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Status>(status, true, out var s))
                parsedStatus = s;

            var filteredTasks = await _taskService.GetAllAsync(parsedPriority, parsedStatus, skip, take, cancellationToken);
            return Ok(filteredTasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(id, cancellationToken);
            if (task == null)
                throw new NotFoundException($"Task with ID {id} was not found.");

            return Ok(task);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskItem task, CancellationToken cancellationToken)
        {
            var created = await _taskService.CreateAsync(task, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskItem task, CancellationToken cancellationToken)
        {
            var updated = await _taskService.UpdateAsync(id, task, cancellationToken);
            if (updated == null)
                throw new NotFoundException($"Task with ID {id} was not found.");
            
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _taskService.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}