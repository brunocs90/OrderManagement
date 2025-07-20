using OrderManagement.Shared.Extensions;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Shared.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EnumValidValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return true;

            return value is Enum enumValue && enumValue.IsValid();
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Invalid value for field {name}";
        }
    }
}