using OrderManagement.Shared.ModelValidators;
using OrderManagement.Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Shared
{
    public static class ValidationHelper
    {
        public static void Validate<T>(T model) where T : class
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelValidator.TryValidate(model, out IEnumerable<ValidationResult> errors))
                throw new ModelValidationException([.. errors.Select(e => new ModelValidationError(e.MemberNames.First(), e.ErrorMessage!))]);
        }
    }
}