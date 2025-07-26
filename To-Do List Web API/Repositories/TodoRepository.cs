using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_List_Web_API.Data;
using To_Do_List_Web_API.Models;

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
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetByDateAsync(DateTime dateTime)
        {
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
