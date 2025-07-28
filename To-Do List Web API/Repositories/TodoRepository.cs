using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using To_Do_List_Web_API.Data;
using To_Do_List_Web_API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace To_Do_List_Web_API.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;
        public TodoRepository(TodoDbContext context)
        {
            _context = context;   
        }
        public async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            if (todoItem.Title.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(todoItem.Title), "Title cannot be null or empty.");
            }
            if (todoItem.IsCompleted == false)
            {
                todoItem.CompletedAt = null;
            }
            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task DeleteAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "ID must be greater than zero.");
            }
            else if (todoItem == null)
            {
                throw new KeyNotFoundException($"Todo Item with ID: {id} not found.");
            }
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            if (_context.TodoItems == null || !_context.TodoItems.Any())
            {
                throw new KeyNotFoundException("Todo items not found/empty.");
            }
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetByDateAsync(DateTime dateTime)
        {
            var isAnyItems = await _context.TodoItems.AnyAsync(item => item.CreatedAt.Date == dateTime.Date);
            if (_context.TodoItems == null || !isAnyItems)
            {
                throw new KeyNotFoundException($"No tasks found for the date: {dateTime.ToShortDateString()}");
            }
            var query = _context.TodoItems
                .Where(item => item.CreatedAt.Date == dateTime.Date).ToListAsync();
            return await query;
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "ID must be greater than zero.");
            }
            else if (todoItem == null)
            {
                throw new KeyNotFoundException($"Todo Item with ID: {id} not found.");
            }

            return todoItem;
        }

        public async Task UpdateAsync(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                throw new ArgumentNullException(nameof(todoItem), "Todo item cannot be null.");
            }
            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task PatchUpdateAsync([FromBody] JsonPatchDocument<TodoItem> todoItem, [FromRoute] int id)
        {
            var existingItem = await _context.TodoItems.FindAsync(id);
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "ID must be greater than zero.");
            } 
            else if (existingItem == null)
            {
                throw new KeyNotFoundException($"Todo Item with ID: {id} not found.");
            }

            todoItem.ApplyTo(existingItem);
            await _context.SaveChangesAsync();
        }
    }
}
