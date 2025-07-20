using OrderManagement.Shared.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OrderManagement.Shared.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CepAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToStrNull()))
                return true;

            var cep = value.ToString()!.Trim();

            // Remove caracteres não numéricos
            cep = Regex.Replace(cep, "[^0-9]", "");

            // Valida se tem 8 dígitos
            return cep.Length == 8;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"CEP is invalid";
        }
    }
}