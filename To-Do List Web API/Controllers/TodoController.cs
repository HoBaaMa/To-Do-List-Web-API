using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using To_Do_List_Web_API.Models;
using To_Do_List_Web_API.Repositories;

namespace To_Do_List_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<TodoController> _logger;
        public TodoController(ITodoRepository todoRepository, ILogger<TodoController> logger)
        {
            this._todoRepository = todoRepository;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            _logger.LogInformation("API call received to display all the tasks.");
            try
            {
                var items = await _todoRepository.GetAllAsync();
                _logger.LogInformation($"{items.Count()} tasks retrieved successfully.");
                //_logger.LogInformation(",{items}");
                return Ok(items);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No tasks found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving tasks.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            _logger.LogInformation($"API call received to display a task with ID: {id}.");
            try
            {
                var item = await _todoRepository.GetByIdAsync(id);
                _logger.LogInformation($"Task with ID: {id} retrieved successfully.");
                return Ok(item);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, $"Invalid ID: {id} provided.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Todo Item with ID: {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving Todo Item with ID: {id}.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetTodoItemsByDate(DateTime date)
        {
            _logger.LogInformation($"API call received to display tasks for the date: {date.ToShortDateString()}");
            try
            {
                var items = await _todoRepository.GetByDateAsync(date);
                _logger.LogInformation($"Tasks for the date {date.ToShortDateString()} retrieved successfully.");
                return Ok(items);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"No tasks found for the date: {date.ToShortDateString()}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving tasks for the date: {date.ToShortDateString()}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(TodoItem todoItem)
        {
            _logger.LogInformation("API call received to add a new task.");
            try
            {
                var createdItem = await _todoRepository.CreateAsync(todoItem);
                _logger.LogInformation("Task: {@TodoItem} created successfully.", todoItem);
                //_logger.LogInformation("Added new Task {@TodoItem}", todoItem);
                return CreatedAtAction(nameof(GetTodoItems), new { id = createdItem.Id, createdItem });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Title cannot be null or empty.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a task.");
                return StatusCode(500, "An internal sever error occurred.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, TodoItem todoItem)
        {
            _logger.LogInformation($"API call received to update task with ID: {id}.");
            if (id == todoItem.Id)
            {
                try
                {
                    await _todoRepository.UpdateAsync(todoItem);
                    _logger.LogInformation($"Task with ID: {id} updated successfully.\n" +
                        "Updated Task: {@TodoItem}", todoItem);
                    return NoContent();
                }
                catch (ArgumentNullException ex)
                {
                    _logger.LogWarning(ex, "Invalid Todo Item provided for update.");
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An unexpected error occurred while updating Todo Item with ID: {id}.");
                    return StatusCode(500, "An internal server error occurred.");
                }
            }
            else
            {
                _logger.LogWarning($"ID mismatch: provided ID {id} does not match Todo Item ID {todoItem.Id}.");
                return BadRequest("Invalid Todo Item.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            _logger.LogInformation($"API call received to delete task with ID: {id}.");
            try
            {
                await _todoRepository.DeleteAsync(id);
                _logger.LogInformation($"Task with ID: {id} deleted successfully.");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Todo Item with ID: {id} not found for deletion.");
                return NotFound(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, $"Invalid ID: {id} provided for deletion.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while deleting Todo Item with ID: {id}.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUpdateAsync([FromBody] JsonPatchDocument<TodoItem> todoItem, [FromRoute] int id)
        {
            _logger.LogInformation($"API call received to partially update task with ID: {id}.");
            try
            {
                await _todoRepository.PatchUpdateAsync(todoItem, id);
                _logger.LogInformation($"Task with ID: {id} partially updated successfully.\n" +
                    "Updated Task: {@TodoItem}", todoItem);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, $"Invalid ID: {id} provided for patch update.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Todo Item with ID: {id} not found for patch update.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while partially updating Todo Item with ID: {id}.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
