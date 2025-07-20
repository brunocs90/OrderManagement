using OrderManagement.Shared.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OrderManagement.Shared.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class TelephoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToStrNull()))
                return true;

            var telefone = Regex.Replace(value.ToString() ?? "", "[^0-9]", "");

            // Deve ter 10 (fixo) ou 11 (celular) dígitos
            // Validação básica de DDD e número fixo (2ª posição entre 2 e 5, exemplo: 2345-6789)
            if (telefone.Length == 10) // Fixo
                return Regex.IsMatch(telefone, @"^[1-9]{2}[2-5][0-9]{7}$");
            else if (telefone.Length == 11) // Celular - Validação básica de celular com nono dígito (9 na 3ª posição)
                return Regex.IsMatch(telefone, @"^[1-9]{2}9[6-9][0-9]{7}$");

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Telephone is invalid";
        }
    }
}