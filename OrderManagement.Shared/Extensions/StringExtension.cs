namespace OrderManagement.Shared.Extensions;

public static class StringExtension
{
    public static string GetOnlyNumbers(this string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? value 
            : new string([.. value.Where(char.IsDigit)]);
    }
}