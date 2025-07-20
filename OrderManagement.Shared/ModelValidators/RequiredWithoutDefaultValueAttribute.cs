using OrderManagement.Shared.Extensions;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Shared.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredWithoutDefaultValueAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value) => value!.HasValue();
    }
}