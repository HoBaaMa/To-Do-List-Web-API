using Azure;
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
        public TodoController(ITodoRepository todoRepository)
        {
            this._todoRepository = todoRepository;
        }
        [HttpGet]
        public async Task <IActionResult> GetTodoItems()
        {
            var items = await _todoRepository.GetAllAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            try
            {
                var item = await _todoRepository.GetByIdAsync(id);
                return Ok(item);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            //if (item == null)
            //{
            //    return NotFound($"Todo Item with ID: {id} is not found.");
            //}
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetTodoItemsByDate(DateTime date)
        {
            var items = await _todoRepository.GetByDateAsync(date);
            if (items == null || !items.Any())
            {
                return NotFound($"No Todo Items found for the date: {date.ToShortDateString()}");
            }
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(TodoItem todoItem)
        {
            var createdItem = await _todoRepository.CreateAsync(todoItem);
            return CreatedAtAction(nameof(GetTodoItems), new { id = createdItem.Id, createdItem });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, TodoItem todoItem)
        {
            if (id == todoItem.Id)
            {
                try
                {
                    await _todoRepository.UpdateAsync(todoItem);
                    return NoContent();
                }
                catch (ArgumentNullException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error updating Todo Item: {ex.Message}");
                }
            }
            else
            {
                return BadRequest("Invalid Todo Item.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            try
            {
                await _todoRepository.DeleteAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUpdateAsync([FromBody] JsonPatchDocument<TodoItem> todoItem, [FromRoute] int id)
        {
            try
            {
                await _todoRepository.PatchUpdateAsync(todoItem, id);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating Todo Item: {ex.Message}");
            }
        }
    }
}
