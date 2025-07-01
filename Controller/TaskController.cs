using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using piton_taskmanagement_api.DTOs.Task;
using piton_taskmanagement_api.Services.Interfaces;
using System.Security.Claims;

namespace piton_taskmanagement_api.Controllers
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

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetAllAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily()
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetDailyAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeekly()
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetWeeklyAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly()
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetMonthlyAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var created = await _taskService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _taskService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _taskService.DeleteAsync(id);
            return NoContent();
        }
    }
}
