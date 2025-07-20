using System.ComponentModel;

namespace OrderManagement.Shared.Extensions;

public static class EnumExtensions
{
    public static T? ToEnumNull<T>(this string value) where T : struct
    {
        return string.IsNullOrEmpty(value) ? null : value.ToEnum<T>();
    }

    public static T? ToEnumNull<T>(this int? value) where T : struct
    {
        return value?.ToEnum<T>();
    }

    public static T ToEnum<T>(this string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length > 1)
            return GetInvalidEnum<T>();

        var result = ConvertToEnum<T>(value.First());
        return result;
    }

    public static T ToEnum<T>(this int value)
    {
        var result = ConvertToEnum<T>(value);
        return result;
    }

    public static T ToEnum<T>(this char value)
    {
        var result = ConvertToEnum<T>(value);
        return result;
    }

    public static bool IsValid(this Enum value)
    {
        var result = Enum.IsDefined(value.GetType(), Convert.ToInt32(value));
        return result;
    }

    public static string GetDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString())!;
        return fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length != 0
            ? attributes[0].Description
            : value.ToString();
    }

    private static T GetInvalidEnum<T>()
    {
        var minValue = Enum.GetValues(typeof(T)).Cast<T>().Select(x => Convert.ToInt32(x)).Min();
        var invalidValue = minValue - 1;
        invalidValue = Math.Min(0, invalidValue);

        var result = ConvertToEnum<T>(invalidValue);
        return result;
    }

    private static T ConvertToEnum<T>(object value)
    {
        var result = (T)Enum.ToObject(typeof(T), value);
        return result;
    }


}