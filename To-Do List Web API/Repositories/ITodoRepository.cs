using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using To_Do_List_Web_API.Models;

namespace To_Do_List_Web_API.Repositories
{
    /// <summary>
    /// Defines the contract for repository operations on TodoItem entities.
    /// </summary>
    public interface ITodoRepository
    {
        /// <summary>
        /// Retrieves all to-do items.
        /// </summary>
        /// <returns>A collection of all <see cref="TodoItem"/> objects.</returns>
        Task<IEnumerable<TodoItem>> GetAllAsync();

        /// <summary>
        /// Retrieves a to-do item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the to-do item.</param>
        /// <returns>The <see cref="TodoItem"/> if found; otherwise, <c>null</c>.</returns>
        Task<TodoItem?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all to-do items created on a specific date.
        /// </summary>
        /// <param name="dateTime">The date to filter to-do items by (compares only the date part).</param>
        /// <returns>A collection of <see cref="TodoItem"/> objects created on the specified date.</returns>
        Task<IEnumerable<TodoItem>> GetByDateAsync(DateTime dateTime);

        /// <summary>
        /// Creates a new to-do item.
        /// </summary>
        /// <param name="todoItem">The to-do item to create.</param>
        /// <returns>The created <see cref="TodoItem"/> with its generated ID and timestamps.</returns>
        Task<TodoItem> CreateAsync(TodoItem todoItem);

        /// <summary>
        /// Updates an existing to-do item.
        /// </summary>
        /// <param name="todoItem">The to-do item with updated values.</param>
        Task UpdateAsync(TodoItem todoItem);

        /// <summary>
        /// Applies a JSON Patch document to a to-do item for partial updates.
        /// </summary>
        /// <param name="todoItem">The JSON Patch document describing the changes.</param>
        /// <param name="id">The unique identifier of the to-do item to update.</param>
        Task PatchUpdateAsync([FromBody] JsonPatchDocument<TodoItem> todoItem, [FromRoute] int id);

        /// <summary>
        /// Deletes a to-do item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the to-do item to delete.</param>
        Task DeleteAsync(int id);
    }
}
