using System.ComponentModel.DataAnnotations;
using To_Do_List_Web_API.Models;

namespace To_Do_List_Web_API.Attributes
{
    /// <summary>
    /// Validates the relationship between <c>IsCompleted</c> and <c>CompletedAt</c> properties of a <see cref="TodoItem"/>.
    /// Ensures that <c>CompletedAt</c> is set when <c>IsCompleted</c> is true, and is null when <c>IsCompleted</c> is false.
    /// </summary>
    /// <param name="value">The value of the property being validated (not used directly).</param>
    /// <param name="validationContext">The context that provides the object instance to validate.</param>
    /// <returns>
    /// <see cref="ValidationResult.Success"/> if the state is valid; otherwise, a <see cref="ValidationResult"/> with an error message.
    /// </returns>
    public class CompletedStateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var todoItem = validationContext.ObjectInstance as TodoItem;
            if (todoItem?.IsCompleted == true && todoItem?.CompletedAt == null)
            {
                return new ValidationResult("CompletedAt must be set when IsCompleted is true.");
            }
            if (todoItem?.IsCompleted == false && todoItem?.CompletedAt != null)
            {
                return new ValidationResult("CompletedAt must be null when IsCompleted is false.");
            }
            return ValidationResult.Success;
        }
    }
}
