using System.ComponentModel.DataAnnotations;
using To_Do_List_Web_API.Models;

namespace To_Do_List_Web_API.Attributes
{
    /// <summary>
    /// Validates that the <c>CreatedAt</c> date of a <see cref="TodoItem"/> is earlier than its <c>CompletedAt</c> date.
    /// Returns a validation error if <c>CreatedAt</c> is after <c>CompletedAt</c>.
    /// </summary>
    /// <param name="value">The value of the property being validated (not used directly).</param>
    /// <param name="validationContext">The context that provides the object instance to validate.</param>
    /// <returns>
    /// <see cref="ValidationResult.Success"/> if <c>CreatedAt</c> is earlier than or equal to <c>CompletedAt</c>,
    /// otherwise a <see cref="ValidationResult"/> with an error message.
    /// </returns>
    public class CompletedCreationDatesValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var todoItem = validationContext.ObjectInstance as TodoItem;
            var comparedValue = todoItem?.CreatedAt.CompareTo(todoItem.CompletedAt);

            if (comparedValue > 0)
            {
                return new ValidationResult("CreatedAt must be earlier than CompletedAt.");
            }

            return ValidationResult.Success;
        }
    }
}
