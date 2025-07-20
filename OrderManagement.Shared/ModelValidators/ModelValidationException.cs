using OrderManagement.Shared.Exceptions;

namespace OrderManagement.Shared.ModelValidators
{
    public class ModelValidationException : DomainException<DomainError>
    {
        public override string Key => nameof(ModelValidationException);

        public ModelValidationException(params DomainError[] errors) : base()
        {
            AddError(errors);
        }
    }

    public class ModelValidationError(string key, string message) : DomainError(key, message)
    {
    }
}