using System.ComponentModel.DataAnnotations;
using RecursiveDataAnnotationsValidation;

namespace OrderManagement.Shared.Validators
{
    public static class ModelValidator
    {
        public static bool IsValid<T>(T model)
        {
            var result = TryValidate(model, out _);
            return result;
        }

        public static bool TryValidate<T>(T model, out IEnumerable<ValidationResult> errors)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var validationResults = new List<ValidationResult>();
            var validator = new RecursiveDataAnnotationValidator();
            var isValid = validator.TryValidateObjectRecursive(model, validationResults);

            errors = validationResults;
            return isValid;
        }
    }
}

