using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using To_Do_List_Web_API.Models;

namespace To_Do_List_Web_API.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task<IEnumerable<TodoItem>> GetByDateAsync(DateTime dateTime);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task UpdateAsync(TodoItem todoItem);
        Task PatchUpdateAsync([FromBody] JsonPatchDocument<TodoItem> todoItem, [FromRoute] int id);
        Task DeleteAsync(int id);
    }
}
